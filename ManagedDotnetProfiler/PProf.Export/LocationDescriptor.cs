// <copyright file="LocationDescriptor.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2022 Datadog, Inc.
// </copyright>

using System;

namespace Datadog.PProf.Export
{
    internal struct LocationDescriptor : IEquatable<LocationDescriptor>
    {
        private readonly string _moduleName;
        private readonly string _frame;

        public LocationDescriptor(string moduleName, string frame)
        {
            _moduleName = moduleName;
            _frame = frame;
        }

        public string ModuleName => _moduleName;

        public string Frame => _frame;

        public bool Equals(LocationDescriptor other)
        {
            return _moduleName == other._moduleName && _frame == other._frame;
        }

        public override bool Equals(object obj)
        {
            return obj is LocationDescriptor other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_moduleName != null ? _moduleName.GetHashCode() : 0) * 397) ^ (_frame != null ? _frame.GetHashCode() : 0);
            }
        }

        public static bool operator ==(LocationDescriptor left, LocationDescriptor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LocationDescriptor left, LocationDescriptor right)
        {
            return !left.Equals(right);
        }
    }
}
