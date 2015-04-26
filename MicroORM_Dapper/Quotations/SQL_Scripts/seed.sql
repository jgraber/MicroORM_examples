INSERT INTO [Person] ([LastName], [FirstName], [Born], [Died])
VALUES ('Einstein', 'Albert', '18790314', '19550418');

INSERT INTO [dbo].[Quote] ([Text], [Year], [Context], [PersonId])
VALUES ('Unthinking respect for authority is the greatest enemy of truth', '1900', NULL, (SELECT TOP 1 Id FROM Person WHERE LastName = 'Einstein'));

INSERT INTO [dbo].[Quote] ([Text], [Year], [Context], [PersonId])
VALUES ('As I have said so many times, God doesn''t play dice with the world.', '1920', NULL, (SELECT TOP 1 Id FROM Person WHERE LastName = 'Einstein'));

INSERT INTO [dbo].[Quote] ([Text], [Year], [Context], [PersonId])
VALUES ('Life is like riding a bicycle. To keep your balance you must keep moving.', '1900', NULL, (SELECT TOP 1 Id FROM Person WHERE LastName = 'Einstein'));




INSERT INTO [Person] ([LastName], [FirstName], [Born], [Died])
VALUES ('Lombardi', 'Vince', '19130611', '19700903');

INSERT INTO [dbo].[Quote] ([Text], [Year], [Context], [PersonId])
VALUES ('If you aren''t fired with enthusiasm, you will be fired with enthusiasm.', NULL, NULL, (SELECT TOP 1 Id FROM Person WHERE LastName = 'Lombardi'));

INSERT INTO [dbo].[Quote] ([Text], [Year], [Context], [PersonId])
VALUES ('We shall play every game to the hilt with every ounce of fiber we have in our bodies.', NULL, NULL, (SELECT TOP 1 Id FROM Person WHERE LastName = 'Lombardi'));




INSERT INTO [Person] ([LastName], [FirstName], [Born], [Died])
VALUES ('Christie', 'Agatha ', '18900915', '19760112');

INSERT INTO [dbo].[Quote] ([Text], [Year], [Context], [PersonId])
VALUES ('Oh dear, I never realized what a terrible lot of explaining one has to do in a murder!', '1956', NULL, (SELECT TOP 1 Id FROM Person WHERE LastName = 'Christie'));




INSERT INTO [Person] ([LastName], [FirstName], [Born], [Died])
VALUES ('DeMarco', 'Tom', '19400820', Null);

INSERT INTO [dbo].[Quote] ([Text], [Year], [Context], [PersonId])
VALUES ('The business of software building isn''t really high-tech at all. It''s most of all a business of talking to each other and writing things down. Those who were making major contributions to the field were more likely to be its best communicators than its best technicians.', '1995', NULL, (SELECT TOP 1 Id FROM Person WHERE LastName = 'DeMarco'));

INSERT INTO [dbo].[Quote] ([Text], [Year], [Context], [PersonId])
VALUES ('A day lost at the beginning of project hurts just as much as a day lost at the end.', '1997', NULL, (SELECT TOP 1 Id FROM Person WHERE LastName = 'DeMarco'));
