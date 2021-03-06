﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FakePersonApi.Models
{
    public class Person
    {
        public string FirstName
        {
            get;
            private set;
        }
        public string LastName
        {
            get;
            private set;
        }
        public string Phone
        {
            get;
            private set;
        }
        public string Personnummer
        {
            get;
            private set;
        }
        public string StreetAddress
        {
            get;
            private set;
        }
        public string CityAddress
        {
            get;
            private set;
        }
        public Person(Dictionary<string, string> p)
        {
            FirstName = p["namn"].Trim().Split(' ')[0];
            LastName = p["namn"].Trim().Split(' ')[1];
            //Address = p["address"].Trim().Replace("&nbsp;", " ");
            if (Regex.IsMatch(p["address"].Trim().Replace("&nbsp;", " "), "\\w+ \\d{1,2}\\d{3}"))
            {
                StreetAddress = Regex.Matches(p["address"].Trim().Replace("&nbsp;", " "), "(\\w+ \\d{1,2})")[0].Captures[0].ToString();
            }
            if (Regex.IsMatch(p["address"].Trim().Replace("&nbsp;", " "), "\\d{3}\\s+\\d{2}\\s+\\w+"))
            {
                CityAddress = Regex.Matches(p["address"].Trim().Replace("&nbsp;", " "), "(\\d{3}\\s+\\d{2}\\s+\\w+)")[0].Captures[0].ToString();
            }

            Personnummer = p["personnummer"].Trim();
            var phn = p["telefonnummer"].Trim();
            Phone = phn.Replace("</", "").Replace("<", "");
        }

        public string ToJSON()
        {
            var result = "{";
            result += $"\"firstname\": \"{FirstName}\",";
            result += $"\"lastname\": \"{LastName}\",";
            result += $"\"streetaddress\": \"{StreetAddress}\",";
            result += $"\"cityaddress\": \"{CityAddress}\",";
            result += $"\"ssn\": \"{Personnummer}\",";
            result += $"\"phone\": \"{Phone}\"";

            return result + "}";
        }

        public static Person FromJSON(string str)
        {

            var toP = new Dictionary<string, string>();
            var attrs = str.Replace("{", "").Replace("}", "").Replace("\"", "").Split(',');
            foreach (var item in attrs)
            {
                var kvPair = item.Split(':');
                var k = kvPair[0];
                if(k == "ssn")
                {
                    k = "personnummer";
                }
                else if(k == "phone")
                {
                    k = "telefonnummer";
                }
                else if(k == "cityaddress" || k == "streetaddress")
                {
                    k = "address";
                }
                else if(k == "firstname" || k == "lastname")
                {
                    k = "namn";
                }
                if (toP.ContainsKey(k))
                {
                    toP[k] += " " + kvPair[1];
                }
                else
                {
                    toP.Add(k, kvPair[1]);
                }
            }


            return new Person(toP);
        }
    }

}