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
using OpenMetaverse;
using OpenMetaverse.StructuredData;
using Universe.Framework.Utilities;

namespace Universe.Framework.Services
{
    public class UserAccount : AllScopeIDImpl, BaseCacheAccount
    {
        public int Created;
        public string Email;
        public string Name { get; set; }
        public UUID PrincipalID { get; set; }
        public int UserFlags = Constants.USER_FLAG_GUEST;
        public int UserLevel = Constants.USER_NORMAL;

        public UserAccount ()
        {
        }

        public UserAccount (UUID principalID)
        {
            PrincipalID = principalID;
        }

        public UserAccount (UUID scopeID, string name, string email)
        {
            PrincipalID = UUID.Random ();
            ScopeID = scopeID;
            Name = name;
            Email = email;
            Created = Util.UnixTimeSinceEpoch ();
        }

        public UserAccount (UUID scopeID, UUID principalID, string name, string email)
        {
            PrincipalID = principalID;
            ScopeID = scopeID;
            Name = name;
            Email = email;
            Created = Util.UnixTimeSinceEpoch ();
        }

        public string FirstName
        {
            get { return Name.Split (' ') [0]; }
        }

        public string LastName
        {
            get {
                string[] split = Name.Split (' ');
                if (split.Length > 1)
                    return Name.Split (' ') [1];
                return "";
            }
        }

        public override OSDMap ToOSD ()
        {
            OSDMap result = new OSDMap ();
            result ["FirstName"] = FirstName;
            result ["LastName"] = LastName;
            result ["Name"] = Name;
            result ["Email"] = Email;
            result ["PrincipalID"] = PrincipalID;
            result ["ScopeID"] = ScopeID;
            result ["AllScopeIDs"] = AllScopeIDs.ToOSDArray ();
            result ["Created"] = Created;
            result ["UserLevel"] = UserLevel;
            result ["UserFlags"] = UserFlags;

            return result;
        }

        public override void FromOSD (OSDMap map)
        {
            Name = map ["FirstName"] + " " + map ["LastName"];
            if (map.ContainsKey ("Name"))
                Name = map ["Name"].AsString ();
            Email = map ["Email"].AsString ();
            if (map.ContainsKey ("PrincipalID"))
                PrincipalID = map ["PrincipalID"];
            if (map.ContainsKey ("ScopeID"))
                ScopeID = map ["ScopeID"];
            if (map.ContainsKey ("AllScopeIDs"))
                AllScopeIDs = ((OSDArray)map ["AllScopeIDs"]).ConvertAll<UUID> (o => o);
            if (map.ContainsKey ("UserLevel"))
                UserLevel = map ["UserLevel"];
            if (map.ContainsKey ("UserFlags"))
                UserFlags = map ["UserFlags"];
            if (map.ContainsKey ("Created"))
                Created = map ["Created"];
        }
    }

    public interface IUserAccountService
    {
        /// <summary>
        /// Returns true if service is local.
        /// This exposes the ConnectorBase field so it can be used by inherited classes
        /// </summary>
        bool IsLocalConnector { get; }

        IUserAccountService InnerService { get; }

        /// <summary>
        ///     Get a user given by UUID
        /// </summary>
        /// <param name="scopeIDs"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        UserAccount GetUserAccount (List<UUID> scopeIDs, UUID userID);

        /// <summary>
        ///     Get a user given by a first and last name
        /// </summary>
        /// <param name="scopeIDs"></param>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <returns></returns>
        UserAccount GetUserAccount (List<UUID> scopeIDs, string FirstName, string LastName);

        /// <summary>
        ///     Get a user given by its full name
        /// </summary>
        /// <param name="scopeIDs"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        UserAccount GetUserAccount (List<UUID> scopeIDs, string Name);

        /// <summary>
        ///     Returns the list of avatars that matches both the search criterion and the scope ID passed
        /// </summary>
        /// <param name="scopeIDs"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        List<UserAccount> GetUserAccounts (List<UUID> scopeIDs, string query);

        /// <summary>
        ///     Returns a paginated list of avatars that matches both the search criterion and the scope ID passed
        /// </summary>
        /// <param name="scopeIDs"></param>
        /// <param name="query"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<UserAccount> GetUserAccounts (List<UUID> scopeIDs, string query, uint? start, uint? count);

        /// <summary>
        ///     Returns a paginated list of avatars that matches both the search criterion and the scope ID passed
        /// </summary>
        /// <param name="scopeIDs"></param>
        /// <param name="level">greater than or equal to clause is used</param>
        /// <param name="flags">bit mask clause is used</param>
        /// <returns></returns>
        List<UserAccount> GetUserAccounts (List<UUID> scopeIDs, int level, int flags);

        /// <summary>
        ///     Returns the number of avatars that match both the search criterion and the scope ID passed
        /// </summary>
        /// <param name="scopeIDs"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        uint NumberOfUserAccounts (List<UUID> scopeIDs, string query);

        /// <summary>
        ///     Store the data given, which replaces the stored data, therefore must be complete.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool StoreUserAccount (UserAccount data);

        /// <summary>
        ///     Cache the given userAccount so that it doesn't have to be queried later
        /// </summary>
        /// <param name="account"></param>
        void CacheAccount (UserAccount account);

        /// <summary>
        ///     Create the user with the given info
        /// </summary>
        /// <param name="name"></param>
        /// <param name="md5password">MD5 hashed password</param>
        /// <param name="email"></param>
        void CreateUser (string name, string md5password, string email);

        /// <summary>
        ///     Create the user with the given info
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="scopeID"></param>
        /// <param name="name"></param>
        /// <param name="md5password">MD5 hashed password</param>
        /// <param name="email"></param>
        /// <returns>The error message (if one exists)</returns>
        string CreateUser (UUID userID, UUID scopeID, string name, string md5password, string email);

        /// <summary>
        ///     Create the user with the given info
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns>The error message (if one exists)</returns>
        string CreateUser (UserAccount account, string password);

        /// <summary>
        ///     Delete a user from the database permanently
        /// </summary>
        /// <param name="userID">The user's ID</param>
        /// <param name="password">The user's password</param>
        /// <param name="archiveInformation">Whether or not we should store the account's name and account information so that the user's information in world does not go null</param>
        /// <param name="wipeFromDatabase">Whether or not we should remove all of the user's data from other locations in the database</param>
        void DeleteUser (UUID userID, string name, string password, bool archiveInformation, bool wipeFromDatabase);

        /// <summary>
        /// Users 'god' level.
        /// </summary>
        /// <returns>The god level description.</returns>
        /// <param name="level">Level.</param>
        string UserGodLevel (int level);
    }

    /// <summary>
    ///     An interface for connecting to the user accounts data-store
    /// </summary>
    public interface IUserAccountData : IUniverseDataPlugin
    {
        string Realm { get; }
        UserAccount[] Get (List<UUID> scopeIDs, string[] fields, string[] values);
        bool Store (UserAccount data);
        bool DeleteAccount (UUID userID, bool archiveInformation);
        UserAccount[] GetUsers (List<UUID> scopeIDs, string query);
        UserAccount[] GetUsers (List<UUID> scopeIDs, string query, uint? start, uint? count);
        UserAccount[] GetUsers (List<UUID> scopeIDs, int level, int flags);
        uint NumberOfUsers (List<UUID> scopeIDs, string query);
    }
}