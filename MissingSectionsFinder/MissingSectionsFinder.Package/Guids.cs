// Guids.cs
// MUST match guids.h
using System;

namespace OmarSalem.MissingSectionsFinder_Package
{
    static class GuidList
    {
        public const string guidMissingSectionsFinder_PackagePkgString = "2c40f27d-f2ec-4652-a203-78f6d95c1cf9";
        public const string guidMissingSectionsFinder_PackageCmdSetString = "579ed196-a889-4022-b647-e2d2c8c1f1ba";
        public const string guidToolWindowPersistanceString = "abb7aac7-ea86-4660-aef7-bede5fcd2ed6";

        public static readonly Guid guidMissingSectionsFinder_PackageCmdSet = new Guid(guidMissingSectionsFinder_PackageCmdSetString);
    };
}