﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using FlightControlWeb.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace FlightControlWeb.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class FlightsController : ControllerBase
//    {
//        private IFlightsManager flightsManager;
//        // GET: api/Flights
//        [HttpGet]
//        public IEnumerable<Flight> GetAllFlights()
//        {
//            return flightsManager.GetAllFlights();
//        }

//        // GET: api/Flights/5
//        [HttpGet("{id}", Name = "Get")]
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST: api/Flights
//        [HttpPost]
//        public void Post([FromBody] string value)
//        {
//        }

//        // PUT: api/Flights/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }

//        // DELETE: api/ApiWithActions/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}