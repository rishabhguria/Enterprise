
CREATE PROCEDURE [dbo].[P_GetExAssignVenue]
As
Select VenueID,VenueName from T_Venue where VenueName = 'Ex&Assign'
