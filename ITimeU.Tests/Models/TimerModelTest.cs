﻿using System;
using System.Threading;
using System.Web.Mvc;
using ITimeU.Controllers;
using ITimeU.DAL;
using ITimeU.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBDD.Dsl.GivenWhenThen;
using TinyBDD.Specification.MSTest;


namespace ITimeU.Tests.Models
{
    /// <summary>
    /// Summary description for TimerModelTest
    /// </summary>
    [TestClass]
    public class TimerModelTest : ScenarioClass
    {

        private TimerModel timerModel = null;

        [TestMethod]
        public void We_Have_A_Timer_Model()
        {
            Scenario("We have a timer model");
            Given("we want to create a timer model");

            When("we instantiate the timer model class",
                () => timerModel = new TimerModel()
            );

            Then("we have a timer model", () => timerModel.ShouldNotBeNull()
            );
        }

        [TestMethod]
        public void The_TimerModel_Has_A_Starttime()
        {
            Given("we have an instance of the timerclass", () => timerModel = new TimerModel());

            When("we we click the startbutton", () => timerModel.Start());

            Then("the timer should have a starttime", () => Assert.IsNotNull(timerModel.StartTime));
        }

        [TestMethod]
        public void Start_Time_Should_Return_Same_Value()
        {
            DateTime? startTime = null;
            Given("We have an instance of timerclass", () => timerModel = new TimerModel());

            When("We click the start button", () =>
            {
                timerModel.Start();
                startTime = timerModel.StartTime;
                Thread.Sleep(100);
            });


            Then("THe timer should return the same value each time", () => startTime.ShouldBe(timerModel.StartTime));
        }

        [TestMethod]
        public void We_Should_Have_A_View_Named_Index()
        {
            Given("we have an instance of timerclass", () => timerModel = new TimerModel());

            When("we want to start the timer");

            Then("a view with a timer should appear", () =>
            {
                TimerController timerController = new TimerController();
                timerController.SetFakeControllerContext();
                ViewResult result = (ViewResult)timerController.Index();
                result.ViewName.ShouldBe("Index");
            });
        }

        [TestMethod]
        public void Start_Time_Should_Return_Same_Value_Even_when_you_press_start_again()
        {
            Object startTime = null;
            Given("We have an instance of timerclass", () => timerModel = new TimerModel());
            When("We click the start button twice", () =>
                {
                    timerModel.Start();
                    startTime = timerModel.StartTime;
                    Thread.Sleep(10);
                    timerModel.Start();
                });
            Then("The timer should return the same value each time", () => startTime.ShouldBe(timerModel.StartTime));



        }

        /*
        [TestMethod]
        public void Should_Get_Exception_When_Getting_StartTime_Before_Timer_Is_Started()
        {
            Object startTime = null;

            Given("we have an instance of timerclass", () => timerModel = new TimerModel());

            When("we fetch the starttime", () =>
                {
                    try
                    {
                        startTime = timerModel.StartTime;
                        true.ShouldBeFalse();
                    }
                    catch (NullReferenceException e)
                    {
                        true.ShouldBeTrue();
                    }
                });
        }
        */

        [TestMethod]
        public void Timer_StartTime_Should_Be_Null_Before_Timer_Is_Started()
        {
            DateTime? startTime = new DateTime();

            Given("we have a timer", () => timerModel = new TimerModel());

            When("we fetch the start time", () => startTime = timerModel.StartTime);

            Then("the start time should be null", () => startTime.ShouldBeNull());
        }

        [TestMethod]
        public void The_Timer_Should_Initially_Not_Be_Started()
        {
            Given("we are going to create a timer");
            When("we create the timer", () => timerModel = new TimerModel());
            Then("the timer should not be started", () => timerModel.IsStarted.ShouldBeFalse());
        }

        [TestMethod]
        public void The_Starttime_Should_Be_Saved_To_The_Database()
        {
            DateTime? startTime = new DateTime();
            Given("we have a timer", () => timerModel = new TimerModel());

            When("we start the time", () =>
            {
                timerModel.Start();
                startTime = timerModel.StartTime;

            });

            Then("the timestamp should be saved to the database", () =>
            {
                var timer = TimerDAL.GetTimerById(timerModel.Id);
                timer.StartTime.HasValue.ShouldBeTrue();
            }
                );

        }
        [TestMethod]
        public void The_Running_Time_Should_Be_Shown()
        {
            Given("we have a timer", () => timerModel = new TimerModel());

            When("the timer is started"); timerModel.Start();

            Then("the running time should have a value", () =>
            {
                TimerController timerController = new TimerController();
                timerController.SetFakeControllerContext();
                ViewResult result = (ViewResult)timerController.Index();
                result.ViewName.ShouldBe("Index");
                result.ViewData["time1"].ShouldNotBeNull();
                
            });
        }


        [TestMethod]
        public void The_start_Time_Should_Be_Stoped()
        {
            DateTime stopTime = new DateTime();

            Given("We have a start timer", () => 
            {
                timerModel = new TimerModel(); 
                timerModel.Start();
            }
                );

            When("We stop the time", () =>
            {
                timerModel.Stop();
            }
                );

            Then("The start time should be stopped", () =>
                {
                    var time = TimerDAL.GetTimerById(timerModel.Id);
                    timerModel.EndTime.HasValue.ShouldBeTrue();
                }
                );
        }

        [TestCleanup]
        public void TestCleanup()
        {

            StartScenario();

        }


    }
}
