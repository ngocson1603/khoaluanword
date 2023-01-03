﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Military_Elite
{
    public class Comando : SpecialisedSoldier, IComando
    {
        public Comando(string id, string firstName, string lastName, double salary, string corps, IList<IMission> missions)
            : base(id, firstName, lastName, salary, corps)
        {
            this.Missions = missions;
        }

        public IList<IMission> Missions { get; }

        public void CompleteMission()
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder($"{base.ToString()}" + Environment.NewLine);
            sb.AppendLine("Missions:");
            sb.AppendLine($"  {string.Join(Environment.NewLine + "  ", this.Missions)}");
            return sb.ToString().Trim();
        }
    }
}