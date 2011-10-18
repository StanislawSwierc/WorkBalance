﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WorkBalance.Domain
{
    public class ActivityTag
    {
        private static readonly Regex c_NameValidator = new Regex(@"^[^\s]+");

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set
            {
                if(m_Name != value)
                {
                    if (!c_NameValidator.IsMatch(value))
                    {
                        throw new FormatException("Tag Name cannot contain any whitespaces");
                    }
                    m_Name = value;
                }
            }
        }
    }
}
