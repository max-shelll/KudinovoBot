﻿using PRTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudinovoBot.DAL.Parameters
{
    public class TestParams : CustomParameters
    {
        public TestParams()
        {
            InitData();
        }
        public DateTime? Data { get; set; }
        public string DataOne { get; set; }
        public string DataTwo { get; set; }
        public override void ClearData()
        {
            DataOne = "";
            DataTwo = "";
            Data = null;
        }

        public override void InitData()
        {
            Data = DateTime.Now;

        }
    }
}