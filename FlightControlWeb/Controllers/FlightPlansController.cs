﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlansController : ControllerBase
    {
        private IFlightsManager flightsManager=new FlightsManager();

        // GET: api/FlightPlans
        [HttpGet]
        public IEnumerable<FlightPlan> GetAllFlight()
        {
            return flightsManager.GetAllFlights();
        }

        // GET: api/FlightPlans/5
        [HttpGet("{id}", Name = "Get")]
        public FlightPlan Get(string id)
        {
            return flightsManager.GetFlight(id);
            
        }

        // POST: api/FlightPlans
        [HttpPost]
        public FlightPlan Post([FromBody] FlightPlan f)
        {
            flightsManager.AddFlight(f);
            return f;
        }

        // PUT: api/FlightPlans/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}