﻿/*
 * Copyright (c) Contributors, http://virtual-planets.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 * For an explanation of the license of each contributor and the content it 
 * covers please see the Licenses directory.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the Virtual Universe Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System.Collections.Generic;
using OpenMetaverse;
using Universe.Framework.DatabaseInterfaces;
using Universe.Framework.SceneInfo;
using Universe.Framework.Servers.HttpServer.Implementation;

namespace Universe.Modules.Web
{
    public class EstateEditPage : IWebInterfacePage
    {
        public string [] FilePath {
            get {
                return new []
                           {
                               "html/admin/estate_edit.html"
                           };
            }
        }

        public bool RequiresAuthentication {
            get { return true; }
        }

        public bool RequiresAdminAuthentication {
            get { return false; }
        }

        public Dictionary<string, object> Fill (WebInterface webInterface, string filename, OSHttpRequest httpRequest,
                                               OSHttpResponse httpResponse, Dictionary<string, object> requestParameters,
                                               ITranslator translator, out string response)
        {
            response = null;
            var vars = new Dictionary<string, object> ();
            var estateConnector = Framework.Utilities.DataManager.RequestPlugin<IEstateConnector> ();

            string estate;

            if (httpRequest.Query.ContainsKey ("EstateID")) {
                estate = httpRequest.Query ["EstateID"].ToString ();
            } else {
                if (requestParameters.ContainsKey ("EstateID")) {
                    estate = requestParameters ["EstateID"].ToString ();
                } else {
                    response = "<h3>Estate details not supplied, redirecting to main page</h3>" +
                        "<script>" +
                        "setTimeout(function() {window.location.href = \"/?page=estate_manager\";}, 1000);" +
                        "</script>";
                    return null;
                }
            }

            var estateid = -1;
            int.TryParse (estate, out estateid);

            if (requestParameters.ContainsKey ("Delete")) {
                response = "Estate details would have been deleted (but not yet).";
                return null;
            }

            if (requestParameters.ContainsKey ("Submit")) {
                var estateSettings = new EstateSettings ();
                if (estateid >= 0)
                    estateSettings = estateConnector.GetEstateSettings (estateid);

                var estateOwner = requestParameters ["EstateOwner"].ToString ();

                estateSettings.EstateName = requestParameters ["EstateName"].ToString (); 
                estateSettings.EstateOwner = UUID.Parse (estateOwner);
                estateSettings.PricePerMeter = int.Parse (requestParameters ["PricePerMeter"].ToString ());
                estateSettings.PublicAccess = requestParameters ["PublicAccess"].ToString () == "1";
                estateSettings.TaxFree = requestParameters ["TaxFree"].ToString () == "1";
                estateSettings.AllowVoice = requestParameters ["AllowVoice"].ToString () == "1";
                estateSettings.AllowDirectTeleport = requestParameters ["AllowDirectTeleport"].ToString () == "1";

                estateConnector.SaveEstateSettings (estateSettings);

                response = "Estate details have been updated." +
                            "<script>" +
                           "setTimeout(function() {window.location.href = \"/?page=estate_manager\";}, 1000);" +
                            "</script>";

                return null;
            }

            if (requestParameters.ContainsKey ("NewEstate")) {
                // blank details for new estate
                vars.Add ("EstateID", "-1");
                vars.Add ("EstateName", "");
                vars.Add ("UserList", WebHelpers.UserSelections (webInterface.Registry, UUID.Zero));
                vars.Add ("PricePerMeter", "");
                vars.Add ("PublicAccess", WebHelpers.YesNoSelection (translator, true));
                vars.Add ("AllowVoice", WebHelpers.YesNoSelection (translator, true));
                vars.Add ("TaxFree", WebHelpers.YesNoSelection (translator, true));
                vars.Add ("AllowDirectTeleport", WebHelpers.YesNoSelection (translator, true));

                vars.Add ("Submit", translator.GetTranslatedString ("AddEstateText"));
            } else {
                // get selected estate details
                var estateSettings = estateConnector.GetEstateSettings (estateid);
                if (estateSettings != null) {
                    vars.Add ("EstateID", estateSettings.EstateID.ToString ());
                    vars.Add ("EstateName", estateSettings.EstateName);
                    vars.Add ("UserList", WebHelpers.UserSelections (webInterface.Registry, estateSettings.EstateOwner));
                    vars.Add ("PricePerMeter", estateSettings.PricePerMeter.ToString ());
                    vars.Add ("PublicAccess", WebHelpers.YesNoSelection (translator, estateSettings.PublicAccess));
                    vars.Add ("AllowVoice", WebHelpers.YesNoSelection (translator, estateSettings.AllowVoice));
                    vars.Add ("TaxFree", WebHelpers.YesNoSelection (translator, estateSettings.TaxFree));
                    vars.Add ("AllowDirectTeleport", WebHelpers.YesNoSelection (translator, estateSettings.AllowDirectTeleport));
                    vars.Add ("Submit", translator.GetTranslatedString ("SaveUpdates"));
                }
            }

            // labels
            vars.Add ("EstateManagerText", translator.GetTranslatedString ("MenuEstateManager"));
            vars.Add ("EstateNameText", translator.GetTranslatedString ("EstateText"));
            vars.Add ("EstateOwnerText", translator.GetTranslatedString ("MenuOwnerTitle"));
            vars.Add ("PricePerMeterText", translator.GetTranslatedString ("PricePerMeterText"));
            vars.Add ("PublicAccessText", translator.GetTranslatedString ("PublicAccessText"));
            vars.Add ("AllowVoiceText", translator.GetTranslatedString ("AllowVoiceText"));
            vars.Add ("TaxFreeText", translator.GetTranslatedString ("TaxFreeText"));
            vars.Add ("AllowDirectTeleportText", translator.GetTranslatedString ("AllowDirectTeleportText"));
            vars.Add ("Cancel", translator.GetTranslatedString ("Cancel"));
            vars.Add ("InfoMessage", "");

            return vars;
        }

        public bool AttemptFindPage (string filename, ref OSHttpResponse httpResponse, out string text)
        {
            text = "";
            return false;
        }
    }
}