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

using OpenMetaverse;

namespace Universe.Framework.Services
{
    public interface ISystemAccountService
    {
        /// <summary>
        /// The governor UUID.
        /// </summary>
        /// <value>The governor UUID.</value>
        UUID GovernorUUID { get;}

        /// <summary>
        ///     The Governor's configured name.
        /// </summary>
        /// <value>The Governor's name.</value>
        string GovernorName { get; }

        /// <summary>
        ///     The System Real Estate owner's UUID
        /// </summary>
        UUID SystemEstateOwnerUUID { get; }

        /// <summary>
        ///     The system Real Estate owner's name
        /// </summary>
        string SystemEstateOwnerName { get; }

        /// <summary>
        ///     The Banker UUID.
        /// </summary>
        /// <value>The Banker UUID.</value>
        UUID BankerUUID { get;}

        /// <summary>
        ///     The Banker's configured name.
        /// </summary>
        /// <value>The Bankers's name.</value>
        string BankerName { get; }

        /// <summary>
        ///     The System Marketplace Owner's UUID
        /// </summary>
        UUID MarketplaceOwnerUUID { get; }

        /// <summary>
        ///     The System Marketplace owner's name
        /// </summary>
        string MarketplaceOwnerName { get; }

        /// <summary>
        /// Get name of a system estate.
        /// </summary>
        /// <returns>The estate owner name.</returns>
        /// <param name="estateID">Estate ID.</param>
        string GetSystemEstateOwnerName(int estateID);

        /// <summary>
        /// Gets a system estate owner UUID.
        /// </summary>
        /// <returns>The estate owner's UUID.</returns>
        /// <param name="estateID">Estate I.</param>
        UUID GetSystemEstateOwner (int estateID);
    }
}