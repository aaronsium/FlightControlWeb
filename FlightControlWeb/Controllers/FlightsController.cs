﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Windows;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private IFlightPlanManager flightsManager;
        private IServerManager serverManager;

        public FlightsController(IFlightPlanManager FManager, IServerManager SManager)
        {
            this.flightsManager = FManager;
            this.serverManager = SManager;
        }

        // GET: api/Flight
        [HttpGet]
        public async Task<IEnumerable<Flight>> GetAllFlight([FromQuery(Name = "relative_to")]string relative_to, [FromQuery(Name = "sync_all")]string sync_all)
        {
            DateTime data = DateTime.ParseExact(relative_to, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            List<Flight> flights = new List<Flight>();
            IEnumerable<FlightPlan> flightPlan = flightsManager.GetAllFlightPlans();

            foreach (FlightPlan item in flightPlan)
            {
                DateTime startTime = DateTime.ParseExact(item.initial_location.date_time, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
                DateTime timespan = startTime;
                double timeInSegment;

                List<Segment> flightSegments = item.segments.ToList();
                for (var i = 0; i < flightSegments.Count; i++)
                {
                    timeInSegment = data.Subtract(timespan).TotalSeconds;
                    timespan = timespan.AddSeconds(flightSegments[i].timespan_seconds);
                    if (startTime <= data && timespan >= data)
                    {
                        double x1, y1, x, y, startY, startX;
                        if (i == 0)
                        {
                            startX = item.initial_location.latitude;
                            startY = item.initial_location.longitude;
                        }
                        else
                        {
                            startX = flightSegments[i - 1].latitude;
                            startY = flightSegments[i - 1].longitude;
                        }

                        x1 = flightSegments[i].latitude - startX;
                        x = startX + timeInSegment * (x1 / flightSegments[i].timespan_seconds);

                        y1 = flightSegments[i].longitude - startY;
                        y = startY + timeInSegment * (y1 / flightSegments[i].timespan_seconds);

                        Flight flight = new Flight
                        {
                            passengers = item.passengers,
                            company_name = item.company_name,
                            flight_id = flightsManager.GetId(item),
                            longitude = y,
                            latitude = x,
                            date_time = item.initial_location.date_time,
                            is_external = false
                        };

                        flights.Add(flight);
                        break;
                    }
                }
            }

            if (Request.QueryString.ToString().Contains("sync_all"))
            {
                IEnumerable<Server> servers = serverManager.GetAllServers();
                if (servers == null)
                {
                    return flights;
                }

                WebClient client = new WebClient();
                foreach (Server item in servers)
                {
                    string request = item.ServerURL + ":" + item.ServerId + "/api/Flights?relative_to=" + relative_to;
                    IEnumerable<Flight> result = await Task.Run(() => DowonloadWebsite(request));

                    foreach (Flight flight in result)
                    {
                        flight.is_external = true;
                    }

                    flights.AddRange(result);
                }
            }
            return flights;
        }

        private IEnumerable<Flight> DowonloadWebsite(string request) {
            
            WebClient client = new WebClient();
            IEnumerable<Flight> output = JsonConvert.DeserializeObject<IEnumerable<Flight>>( client.DownloadString(request));

            return output;
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            flightsManager.DeleteFlight(id);
        }
    }
}
