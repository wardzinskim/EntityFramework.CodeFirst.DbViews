CREATE VIEW PeopleViews AS
SELECT p.Id AS 'PersonId', p.Name, p.Surname, a.City, a.Street
FROM People p
LEFT JOIN Addresses a ON a.Id = p.AddressId