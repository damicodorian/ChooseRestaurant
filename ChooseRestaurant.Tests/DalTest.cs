using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChooseRestaurant.Models;
using System.Data.Entity;
using System.Collections.Generic;

namespace ChooseRestaurant.Tests
{
    [TestClass]
    public class DalTests
    {
        private IDal dal;

        // Appelée avant chaque test
        [TestInitialize]
        public void Init_BeforeEachTest()
        {
            IDatabaseInitializer<DbContextClass> init = new DropCreateDatabaseAlways<DbContextClass>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new DbContextClass());

            dal = new Dal();
        }

        // Appelée après chaque test
        [TestCleanup]
        public void AfterEachTest()
        {
            dal.Dispose();
        }

        [TestMethod]
        public void CreateRestaurante()
        {
            dal.CreateRestaurant("La bonne fourchette", "0102030405");
            List<Restaurant> restaurants = dal.GetAllRestaurants();

            Assert.IsNotNull(restaurants);
            Assert.AreEqual(1, restaurants.Count);
            Assert.AreEqual("La bonne fourchette", restaurants[0].Name);
            Assert.AreEqual("0102030405", restaurants[0].Phone);
        }

        [TestMethod]
        public void ModifyRestaurant_CreationNewRestaurantAndModifyPhoneAndNumber()
        {
            dal.CreateRestaurant("La bonne fourchette", "0102030405");
            dal.ModifyRestaurant(1, "La bonne cuillère", null);

            List<Restaurant> restaurants = dal.GetAllRestaurants();
            Assert.IsNotNull(restaurants);
            Assert.AreEqual(1, restaurants.Count);
            Assert.AreEqual("La bonne cuillère", restaurants[0].Name);
            Assert.IsNull(restaurants[0].Phone);
        }

        [TestMethod]
        public void RestaurantExists_WithCreationNewRestauraunt()
        {
            dal.CreateRestaurant("La bonne fourchette", "0102030405");

            bool exists = dal.RestaurantExists("La bonne fourchette");

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void RestaurantExists_WithInexistantRestauraunt()
        {
            bool exists = dal.RestaurantExists("La bonne fourchette");

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void GetUser_InexistantUser()
        {
            UserClass user = dal.GetUser(1);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUser_WithWrongIdType()
        {
            UserClass user = dal.GetUser("abc");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void AddUserr_NewUserAndGetIt()
        {
            dal.AddUser("Nouvel utilisateur", "12345");

            UserClass user = dal.GetUser(1);

            Assert.IsNotNull(user);
            Assert.AreEqual("Nouvel utilisateur", user.Name);

            user = dal.GetUser("1");

            Assert.IsNotNull(user);
            Assert.AreEqual("Nouvel utilisateur", user.Name);
        }

        [TestMethod]
        public void Authentification_CorrectLoginAndPassword()
        {
            dal.AddUser("Nouvel utilisateur", "12345");

            UserClass user = dal.Authentification("Nouvel utilisateur", "12345");

            Assert.IsNotNull(user);
            Assert.AreEqual("Nouvel utilisateur", user.Name);
        }

        [TestMethod]
        public void Authentification_IncorrectPassword()
        {
            dal.AddUser("Nouvel utilisateur", "12345");
            UserClass user = dal.Authentification("Nouvel utilisateur", "0");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Authentification_IncorrectLongin()
        {
            dal.AddUser("Nouvel utilisateur", "12345");
            UserClass user = dal.Authentification("Nouvel", "12345");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Authentification_IncorrectLonginAndPassword()
        {
            UserClass user = dal.Authentification("Nouvel utilisateur", "12345");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void HasAlreadyVoted_WithWrongTypeParameter()
        {
            bool vote = dal.HasAlreadyVoted(1, "abc");
            Assert.IsFalse(vote);
        }

        [TestMethod]
        public void HasAlreadyVoted_UserHasNotVoted()
        {
            int idSurvey = dal.CreateSurvey();
            int idUser = dal.AddUser("Nouvel utilisateur", "12345");

            bool vote = dal.HasAlreadyVoted(idSurvey, idUser.ToString());

            Assert.IsFalse(vote);
        }

        [TestMethod]
        public void HasAlreadyVoted_UserHasVoted()
        {
            int idSurvey = dal.CreateSurvey();
            int idUser = dal.AddUser("Nouvel utilisateur", "12345");
            dal.CreateRestaurant("La bonne fourchette", "0102030405");
            dal.AddVote(idSurvey, 1, idUser);

            bool vote = dal.HasAlreadyVoted(idSurvey, idUser.ToString());

            Assert.IsTrue(vote);
        }

        [TestMethod]
        public void GetResults()
        {
            int idSurvey = dal.CreateSurvey();
            int idUser1 = dal.AddUser("Utilisateur1", "12345");
            int idUser2 = dal.AddUser("Utilisateur2", "12345");
            int idUser3 = dal.AddUser("Utilisateur3", "12345");

            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");

            dal.AddVote(idSurvey, 1, idUser1);
            dal.AddVote(idSurvey, 3, idUser1);
            dal.AddVote(idSurvey, 4, idUser1);
            dal.AddVote(idSurvey, 1, idUser2);
            dal.AddVote(idSurvey, 1, idUser3);
            dal.AddVote(idSurvey, 3, idUser3);

            List<Results> results = dal.GetResults(idSurvey);

            Assert.AreEqual(3, results[0].NumberOfVotes);
            Assert.AreEqual("Resto pinière", results[0].Name);
            Assert.AreEqual("0102030405", results[0].Phone);
            Assert.AreEqual(2, results[1].NumberOfVotes);
            Assert.AreEqual("Resto mate", results[1].Name);
            Assert.AreEqual("0102030405", results[1].Phone);
            Assert.AreEqual(1, results[2].NumberOfVotes);
            Assert.AreEqual("Resto ride", results[2].Name);
            Assert.AreEqual("0102030405", results[2].Phone);
        }

        [TestMethod]
        public void ObtenirLesResultats_AvecDeuxSondages_RetourneBienLesBonsResultats()
        {
            int idSurvey1 = dal.CreateSurvey();
            int idUser1 = dal.AddUser("Utilisateur1", "12345");
            int idUser2 = dal.AddUser("Utilisateur2", "12345");
            int idUser3 = dal.AddUser("Utilisateur3", "12345");
            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");
            dal.AddVote(idSurvey1, 1, idUser1);
            dal.AddVote(idSurvey1, 3, idUser1);
            dal.AddVote(idSurvey1, 4, idUser1);
            dal.AddVote(idSurvey1, 1, idUser2);
            dal.AddVote(idSurvey1, 1, idUser3);
            dal.AddVote(idSurvey1, 3, idUser3);

            int idSurvey2 = dal.CreateSurvey();
            dal.AddVote(idSurvey2, 2, idUser1);
            dal.AddVote(idSurvey2, 3, idUser1);
            dal.AddVote(idSurvey2, 1, idUser2);
            dal.AddVote(idSurvey2, 4, idUser3);
            dal.AddVote(idSurvey2, 3, idUser3);

            List<Results> results1 = dal.GetResults(idSurvey1);
            List<Results> results2 = dal.GetResults(idSurvey2);

            Assert.AreEqual(3, results1[0].NumberOfVotes);
            Assert.AreEqual("Resto pinière", results1[0].Name);
            Assert.AreEqual("0102030405", results1[0].Phone);
            Assert.AreEqual(2, results1[1].NumberOfVotes);
            Assert.AreEqual("Resto mate", results1[1].Name);
            Assert.AreEqual("0102030405", results1[1].Phone);
            Assert.AreEqual(1, results1[2].NumberOfVotes);
            Assert.AreEqual("Resto ride", results1[2].Name);
            Assert.AreEqual("0102030405", results1[2].Phone);

            Assert.AreEqual(1, results2[0].NumberOfVotes);
            Assert.AreEqual("Resto pinambour", results2[0].Name);
            Assert.AreEqual("0102030405", results2[0].Phone);
            Assert.AreEqual(2, results2[1].NumberOfVotes);
            Assert.AreEqual("Resto mate", results2[1].Name);
            Assert.AreEqual("0102030405", results2[1].Phone);
            Assert.AreEqual(1, results2[2].NumberOfVotes);
            Assert.AreEqual("Resto pinière", results2[2].Name);
            Assert.AreEqual("0102030405", results2[2].Phone);
        }
    }
}
