  DECLARE @__entity_equality_user_0_Id nvarchar = '00000000-ffff-ffff-ffff-ffffffffffff'
  
  SELECT [g].[GearItemId], [g].[ApplicationUserId], [g].[Cost], [g].[DatePurchased], [g].[Description], [g].[GearTypeId], [g].[PhotoUrl], [g].[SerialNumber], [g].[Title], [g].[TripId], [t].[TripGearId], [t].[GearItemId], [t].[IsPacked], [t].[TripId]
      FROM [GearItems] AS [g]
      LEFT JOIN [AspNetUsers] AS [a] ON [g].[ApplicationUserId] = [a].[Id]
      LEFT JOIN [TripGear] AS [t] ON [g].[GearItemId] = [t].[GearItemId]
      WHERE ((([a].[Id] = @__entity_equality_user_0_Id) AND ([a].[Id] IS NOT NULL AND @__entity_equality_user_0_Id IS NOT NULL)) OR ([a].[Id] IS NULL AND @__entity_equality_user_0_Id IS NULL)) AND NOT (EXISTS (
          SELECT 1
          FROM [TripGear] AS [t0]
          WHERE [g].[GearItemId] = [t0].[GearItemId]))
      ORDER BY [g].[GearItemId], [t].[TripGearId]

SELECT * FROM GearItems gi LEFT JOIN TripGear tg ON gi.GearItemId = tg.GearItemId

DELETE FROM TripGear

DELETE FROM Trips Where TripId = 3

