﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SquirrelFinder.Nuts
{
    public class SitefinityNut : Nut
    {
        public SitefinityNut() { }

        public SitefinityNut(Uri url) : base(url) { }

        public override HttpStatusCode Peek(int timeout = 5000)
        {
            NutState currentState = State;
            var request = WebRequest.Create(Url);
            request.Timeout = timeout;
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.ResponseUri.ToString() != Url.ToString())
                        State = NutState.Searching;
                    else
                        State = NutState.Found;

                    if (currentState != State)
                        HasShownMessage = false;

                    LastResponse = response.StatusCode;

                    OnNutChanged(new NutEventArgs(this));
                    return response.StatusCode;
                }
            }
            catch
            {
                State = NutState.Lost;
            }

            if (currentState != State)
                HasShownMessage = false;

            OnNutChanged(new NutEventArgs(this));
            return HttpStatusCode.NotFound;
        }

        public override string GetBalloonTipInfo()
        {
            return $"The '{Title}' nut says it's {State.ToString()} - {Guid.NewGuid()}";
        }

        public override string GetBalloonTipTitle()
        {
            return $"Sitefinity Nut Activity ({Title})";
        }
    }
}
