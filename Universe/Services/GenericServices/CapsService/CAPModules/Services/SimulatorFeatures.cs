/*
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
using System.IO;
using OpenMetaverse.StructuredData;
using Universe.Framework.Servers.HttpServer;
using Universe.Framework.Servers.HttpServer.Implementation;
using Universe.Framework.Services;
using Universe.Framework.Utilities;

namespace Universe.Services
{
    public class SimulatorFeatures : ICapsServiceConnector
    {
        IRegionClientCapsService m_service;

        // Configuration
        static List<string> m_lastNames = new List<string> ();
        static List<string> m_fullNames = new List<string> ();

        #region ICapsServiceConnector Members

        public void RegisterCaps (IRegionClientCapsService service)
        {
            m_service = service;

            // retrieve our god's if needed
            InitGodNames ();

            m_service.AddStreamHandler ("SimulatorFeatures",
                new GenericStreamHandler ("GET", m_service.CreateCAPS ("SimulatorFeatures", ""), SimulatorFeaturesCAP));
        }

        public void DeregisterCaps ()
        {
            m_service.RemoveStreamHandler ("SimulatorFeatures", "GET");
        }

        public void EnteringRegion ()
        {
        }

        #endregion

        byte [] SimulatorFeaturesCAP (string path, Stream request, OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            OSDMap data = new OSDMap ();
            // AvatarHoverHeight enabled
            data ["AvatarHoverHeightEnabled"] = true;

            // MaxMaterialsPerTransaction enabled
            data ["MaxMaterialsPerTransaction"] = 50;

            data ["MeshRezEnabled"] = true;
            data ["MeshUploadEnabled"] = true;
            data ["MeshXferEnabled"] = true;
            data ["PhysicsMaterialsEnabled"] = true;

            OSDMap typesMap = new OSDMap ();

            typesMap ["convex"] = true;
            typesMap ["none"] = true;
            typesMap ["prim"] = true;

            data ["PhysicsShapeTypes"] = typesMap;

            // some additional features
            data ["god_names"] = GodNames (httpRequest);

            //Send back data
            return OSDParser.SerializeLLSDXmlBytes (data);
        }

        #region helpers

        void InitGodNames ()
        {
            if (m_fullNames.Count > 0)
                return;

            IUserAccountService userService = m_service.Registry.RequestModuleInterface<IUserAccountService> ();
            var gods = userService.GetUserAccounts (null, "*");
            if (gods != null) {
                foreach (UserAccount user in gods)
                    if (user.UserLevel >= Constants.USER_GOD_LIASON) {
                        m_lastNames.Add (user.LastName);
                        m_fullNames.Add (user.Name);
                    }
            }
        }

        OSDMap GodNames (OSHttpRequest httpRequest)
        {
            OSDMap namesmap = new OSDMap ();
            if (httpRequest.Query.ContainsKey ("god_names")) {
                OSD nmap = httpRequest.Query ["god_names"].ToString ();
                namesmap = (OSDMap)nmap;
            }

            OSDArray fnames = new OSDArray ();
            foreach (string name in m_fullNames) {
                fnames.Add (name);
            }
            namesmap ["full_names"] = fnames;

            OSDArray lnames = new OSDArray ();
            foreach (string name in m_lastNames) {
                lnames.Add (name);
            }
            namesmap ["last_names"] = lnames;

            return namesmap;
        }

        void CameraOnllyModeRequest (OSHttpRequest httpRequest)
        {
            OSDMap extrasMap = new OSDMap ();
            if (httpRequest.Query.ContainsKey ("OpenSimExtras")) {
                OSD nmap = httpRequest.Query ["OpenSimExtras"].ToString ();
                extrasMap = (OSDMap)nmap;
            }

            extrasMap ["camera-only-mode"] = OSDMap.FromString ("true");
        }

        #endregion
    }
}