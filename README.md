## USAGE

GET /api/Person
    Response: 1 (ONE) JSON-encoded Person with these attributes: 
    + firstname
    + lastname
    + streetaddress
    + cityaddress
    + ssn
    + phone

GET /api/Person/{num}
    Response: 1 (ONE) JSON-array with {num} Person-objects.