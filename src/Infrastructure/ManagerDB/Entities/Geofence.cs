﻿// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using Common.Infrastructure;
using NetTopologySuite.Geometries;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class Geofence(Guid geofenceId, Polygon geom, Guid accountId, string name, string? description, short color, short type, bool active) : BaseAuditableEntity
{
    public Guid GeofenceId { get; set; } = geofenceId;
    public Guid AccountId { get; set; } = accountId;
    public Polygon Geom { get; set; } = geom;
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public short Color { get; set; } = color;
    public short Type { get; set; } = type;
    public bool Active { get; set; } = active;

}
