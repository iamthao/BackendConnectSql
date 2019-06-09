ALTER TABLE Location 
ADD  StateOrProvinceOrRegion varchar(100)

ALTER TABLE Location 
ADD IdCountryOrRegion int
FOREIGN KEY (IdCoutryOrRegion) REFERENCES CountryOrRegion(Id)