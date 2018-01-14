using MvcApplication2.Filters;
using MvcApplication2.Models;
using MvcApplication2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication2.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Test/

        public string GetString()
        {
            return "Hello World is old now. It’s time for wassup bro ;)";
        }

        [Authorize]
        public ActionResult Index()
        {
            EmployeeListViewModel employeeListViewModel = new EmployeeListViewModel();
            employeeListViewModel.UserName = User.Identity.Name; //New Line
            EmployeeBusinessLayer empBal = new EmployeeBusinessLayer();
            List<Employee> employees = empBal.GetEmployees();

            List<EmployeeViewModel> empViewModels = new List<EmployeeViewModel>();

            foreach (Employee emp in employees)
            {
                EmployeeViewModel empViewModel = new EmployeeViewModel();
                empViewModel.EmployeeName = emp.FirstName + " " + emp.LastName;
                empViewModel.Salary = emp.Salary.ToString("C");
                //if (emp.Salary.HasValue)
                //    empViewModel.Salary = ((int)emp.Salary).ToString("C");
                //else
                //    empViewModel.Salary = 0.ToString("C");    //empViewModel.Salary = "";
                if (emp.Salary > 15000)
                {
                    empViewModel.SalaryColor = "yellow";
                }
                else
                {
                    empViewModel.SalaryColor = "green";
                }
                empViewModels.Add(empViewModel);
            }
            employeeListViewModel.Employees = empViewModels;
            employeeListViewModel.FooterData = new FooterViewModel();
            employeeListViewModel.FooterData.CompanyName = "StepByStepSchools"; //Can be set to dynamic value
            employeeListViewModel.FooterData.Year = DateTime.Now.Year.ToString();
            return View("Index", employeeListViewModel);
        }

        [AdminFilter]
        public ActionResult AddNew()
        {
         CreateEmployeeViewModel employeeListViewModel = new CreateEmployeeViewModel();
         employeeListViewModel.FooterData = new FooterViewModel();
         employeeListViewModel.FooterData.CompanyName = "StepByStepSchools"; //Can be set to dynamic value
         employeeListViewModel.FooterData.Year = DateTime.Now.Year.ToString();
         employeeListViewModel.UserName = User.Identity.Name;  //New Line
         return View("CreateEmployee", employeeListViewModel);
        }

        public ActionResult GetAddNewLink()
        {
            if (Convert.ToBoolean(Session["IsAdmin"]))
            {
                return PartialView("AddNewLink");
            }
            else
            {
                return new EmptyResult();
            }
        }

        [AdminFilter]
        public ActionResult SaveEmployee(Employee e, string BtnSubmit)
        {
            switch (BtnSubmit)
            {
                case "Save Employee":
                    if (ModelState.IsValid)
                    {
                        EmployeeBusinessLayer empBal = new EmployeeBusinessLayer();
                        empBal.SaveEmployee(e);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        CreateEmployeeViewModel vm = new CreateEmployeeViewModel();
                        vm.FirstName = e.FirstName;
                        vm.LastName = e.LastName;
                        vm.Salary = e.Salary.ToString();

                        vm.FooterData = new FooterViewModel();
                        vm.FooterData.CompanyName = "StepByStepSchools";//Can be set to dynamic value
                        vm.FooterData.Year = DateTime.Now.Year.ToString();
                        vm.UserName = User.Identity.Name; //New Line
                        //if (e.Salary.HasValue)
                        //    vm.Salary = e.Salary.ToString();
                        //else
                        //    vm.Salary = ModelState["Salary"].Value.AttemptedValue;
                        return View("CreateEmployee", vm);    // Day 4 Change - Passing e here
                    }
                case "Cancel":
                    return RedirectToAction("Index");
            }
            return new EmptyResult();
        }
    }
}
