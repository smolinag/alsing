﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alsing.Net.Messages;

namespace Alsing.Irc.Messages
{
    public class IrcErrorMessage : ConnectionBaseMessage<IrcConnection>
    {
        public string Message { get; set; }
    }
}
