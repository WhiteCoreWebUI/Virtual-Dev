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

using System;
using System.Collections.Generic;
using Universe.Framework.Utilities;

namespace Universe.DataManager.Migration.Migrators.Groups
{
	public class GroupsMigrator_0 : Migrator
	{
		public GroupsMigrator_0 ()
		{
			Version = new Version (0, 0, 0);
			MigrationName = "Groups";

			Schema = new List<SchemaDefinition> ();

			// Group Agent
			AddSchema ("group_agent", ColDefs (
				ColDef ("AgentID", ColumnTypes.String50),
				ColDef ("ActiveGroupID", ColumnTypes.String50)
			), IndexDefs (
				IndexDef (new string[1] { "AgentID" }, IndexType.Primary)
			));

			// Group Data
			AddSchema ("group_data", ColDefs (
				ColDef ("GroupID", ColumnTypes.String50),
				ColDef ("Name", ColumnTypes.String50),
				ColDef ("Charter", ColumnTypes.Text),
				ColDef ("InsigniaID", ColumnTypes.String50),
				ColDef ("FounderID", ColumnTypes.String50),
				ColDef ("MembershipFee", ColumnTypes.String50),
				ColDef ("OpenEnrollment", ColumnTypes.String50),
				ColDef ("ShowInList", ColumnTypes.String50),
				ColDef ("AllowPublish", ColumnTypes.String50),
				ColDef ("MaturePublish", ColumnTypes.String50),
				ColDef ("OwnerRoleID", ColumnTypes.String50)
			), IndexDefs (
				IndexDef (new string[1] { "GroupID" }, IndexType.Primary),
				IndexDef (new string[1] { "Name" }, IndexType.Unique)
			));

			// Group Invites
			AddSchema ("group_invite", ColDefs (
				ColDef ("InviteID", ColumnTypes.String50),
				ColDef ("GroupID", ColumnTypes.String50),
				ColDef ("RoleID", ColumnTypes.String50),
				ColDef ("AgentID", ColumnTypes.String50),
				ColDef ("TMStamp", ColumnTypes.String50),
				ColDef ("FromAgentName", ColumnTypes.String50)
			), IndexDefs (
				IndexDef (new string[4] { "InviteID", "GroupID", "RoleID", "AgentID" },
					IndexType.Primary),
				IndexDef (new string[2] { "AgentID", "InviteID" }, IndexType.Index)
			));

			// Group Membership
			AddSchema ("group_membership", ColDefs (
				ColDef ("GroupID", ColumnTypes.String50),
				ColDef ("AgentID", ColumnTypes.String50),
				ColDef ("SelectedRoleID", ColumnTypes.String50),
				ColDef ("Contribution", ColumnTypes.String45),
				ColDef ("ListInProfile", ColumnTypes.String45),
				ColDef ("AcceptNotices", ColumnTypes.String45)
			), IndexDefs (
				IndexDef (new string[2] { "GroupID", "AgentID" }, IndexType.Primary),
				IndexDef (new string[1] { "AgentID" }, IndexType.Index)
			));

			// Group Notices
			AddSchema ("group_notice", ColDefs (
				ColDef ("GroupID", ColumnTypes.String50),
				ColDef ("NoticeID", ColumnTypes.String50),
				ColDef ("Timestamp", ColumnTypes.String50),
				ColDef ("FromName", ColumnTypes.String255),
				ColDef ("Subject", ColumnTypes.String255),
				ColDef ("Message", ColumnTypes.Text),
				ColDef ("HasAttachment", ColumnTypes.String50),
				ColDef ("ItemID", ColumnTypes.String50),
				ColDef ("AssetType", ColumnTypes.String50),
				ColDef ("ItemName", ColumnTypes.String50)
			), IndexDefs (
				IndexDef (new string[3] { "GroupID", "NoticeID", "Timestamp" },
					IndexType.Primary)
			));

			// Group Role Membership
			AddSchema ("group_role_membership", ColDefs (
				ColDef ("GroupID", ColumnTypes.String50),
				ColDef ("RoleID", ColumnTypes.String50),
				ColDef ("AgentID", ColumnTypes.String50)
			), IndexDefs (
				IndexDef (new string[3] { "GroupID", "RoleID", "AgentID" },
					IndexType.Primary),
				IndexDef (new string[2] { "AgentID", "GroupID" }, IndexType.Index)
			));

			// Group Roles
			AddSchema ("group_roles", ColDefs (
				ColDef ("GroupID", ColumnTypes.String50),
				ColDef ("RoleID", ColumnTypes.String50),
				ColDef ("Name", ColumnTypes.String255),
				ColDef ("Description", ColumnTypes.String255),
				ColDef ("Title", ColumnTypes.String255),
				ColDef ("Powers", ColumnTypes.String50)
			), IndexDefs (
				IndexDef (new string[2] { "GroupID", "RoleID" }, IndexType.Primary),
				IndexDef (new string[1] { "RoleID" }, IndexType.Index)
			));

			// Group Proposals - They currently are stored as generic data so might reconsider this.
			AddSchema ("group_proposals", ColDefs (
				ColDef ("GroupID", ColumnTypes.String50),
				ColDef ("Duration", ColumnTypes.Integer11),
				ColDef ("Majority", ColumnTypes.Float),
				ColDef ("Text", ColumnTypes.Text),
				ColDef ("Quorum", ColumnTypes.Integer11),
				ColDef ("Session", ColumnTypes.String50),
				ColDef ("BallotInitiator", ColumnTypes.String50),
				ColDef ("Created", ColumnTypes.DateTime),
				ColDef ("Ending", ColumnTypes.DateTime),
				ColDef ("VoteID", ColumnTypes.String50)));

			// Group Proposal Votes
			AddSchema ("group_proposals_votes", ColDefs (
				ColDef ("ProposalID", ColumnTypes.String50),
				ColDef ("UserID", ColumnTypes.String50),
				ColDef ("Vote", ColumnTypes.String10)));

			// groupbans
			AddSchema ("group_bans", ColDefs (
				ColDef ("GroupID", ColumnTypes.String50),
				ColDef ("AgentID", ColumnTypes.String50),
				ColDef ("BanTime", ColumnTypes.DateTime)));
		}

		protected override void DoCreateDefaults (IDataConnector genericData)
		{
			EnsureAllTablesInSchemaExist (genericData);
		}

		protected override bool DoValidate (IDataConnector genericData)
		{
			return TestThatAllTablesValidate (genericData);
		}

		protected override void DoMigrate (IDataConnector genericData)
		{
			DoCreateDefaults (genericData);
		}

		protected override void DoPrepareRestorePoint (IDataConnector genericData)
		{
			CopyAllTablesToTempVersions (genericData);
		}
	}
}