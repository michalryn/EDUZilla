﻿using EDUZilla.Models;
using EDUZilla.Services;
using EDUZilla.ViewModels.Class;
using EDUZilla.ViewModels.Role;
using EDUZilla.ViewModels.Student;
using EDUZilla.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EDUZilla.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        #region Properties

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StudentService _studentService;
        private readonly ClassService _classService;

        #endregion

        #region Constructors

        public AdministratorController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, StudentService studentService, ClassService classService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _studentService = studentService;
            _classService = classService;
        }

        #endregion

        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            IList<IdentityRole> roleList = await Task.Run(() => _roleManager.Roles.ToList());

            string[] roleNames = new string[roleList.Count()];
            for(int i = 0; i < roleList.Count(); i++)
            {
                roleNames[i] = roleList[i].Name;
            }

            return View(new CreateUserViewModel
            {
                Roles = roleNames
            });
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel createUserViewModel)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user;
                if(createUserViewModel.Role == "Student")
                {
                    user = UserStore<Student>.CreateUser();
                }
                else if (createUserViewModel.Role == "Teacher")
                {
                    user = UserStore<Teacher>.CreateUser();
                }
                else if (createUserViewModel.Role == "Parent")
                {
                    user = UserStore<Parent>.CreateUser();
                }
                else
                {
                    user = UserStore<ApplicationUser>.CreateUser();
                }

                user.UserName = createUserViewModel.Email;
                user.Email = createUserViewModel.Email;
                user.FirstName = createUserViewModel.FirstName;
                user.LastName = createUserViewModel.LastName;

                IdentityResult result = await _userManager.CreateAsync(user, createUserViewModel.Password);

                if(result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(user, createUserViewModel.Role);
                    if (!result.Succeeded)
                    {
                        foreach (IdentityError error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            return await CreateUser();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AssignStudentToClass()
        {
            IList<StudentListViewModel> studentsList = await _studentService.GetStudentsListAsync();

            return View(studentsList);
        }

        /*[HttpGet]
        public async Task<IActionResult> ViewClasses()
        {
            List<ClassListViewModel> list = await _classService.GetClassesListAsync();

            return View(list);
        }*/

        [HttpGet]
        public async Task<IActionResult> ViewStudents()
        {
            IList<StudentListViewModel> studentsList = await _studentService.GetStudentsListAsync();

            return View(studentsList);
        }

        private static class UserStore<T>
        {
            public static T CreateUser()
            {
                try
                {
                    return Activator.CreateInstance<T>();
                }
                catch
                {
                    throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. ");
                }
            }
        }

        #endregion
    }
}
