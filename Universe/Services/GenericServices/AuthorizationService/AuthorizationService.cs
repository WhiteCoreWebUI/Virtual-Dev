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

using Nini.Config;
using Universe.Framework.ConsoleFramework;
using Universe.Framework.Modules;
using Universe.Framework.PresenceInfo;
using Universe.Framework.SceneInfo;
using Universe.Framework.Services;

namespace Universe.Services
{
    public class AuthorizationService : IAuthorizationService, IService
    {
        private IRegistryCore m_registry;

        #region IAuthorizationService Members

        public bool IsAuthorizedForRegion(GridRegion region, AgentCircuitData agent, bool isRootAgent, out string reason)
        {
            ISceneManager manager = m_registry.RequestModuleInterface<ISceneManager>();
            IScene scene = manager == null ? null : manager.Scenes.Find((s) => s.RegionInfo.RegionID == region.RegionID);
            if (scene != null)
            {
                //Found the region, check permissions
                return scene.Permissions.AllowedIncomingAgent(agent, isRootAgent, out reason);
            }
            reason = "Not Authorized as region does not exist.";
            return false;
        }

        #endregion

        #region IService Members

        public void Initialize(IConfigSource config, IRegistryCore registry)
        {
            registry.RegisterModuleInterface<IAuthorizationService>(this);
            m_registry = registry;

            MainConsole.Instance.Debug("[AuthorizationService]: Local Authorization service enabled");
        }

        public void Start(IConfigSource config, IRegistryCore registry)
        {
        }

        public void FinishedStartup()
        {
        }

        #endregion
    }
}