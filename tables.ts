// public virtual DbSet<CitizenDetailsResultModel> CitizenDetailsResultModels { get; set; }

// modelBuilder.Entity<CitizenDetailsResultModel>().HasNoKey();



const formElements = [
  {
    "section": "Location Details",
    "fields":[
      {"type":"select","label":"Select District Social Welfare Officer","name":"District","isFormSpecific":true},
    ]
  },
  {
    "section": "Applicant Details",
    "fields":[
      {"type":"text","label":"Applicant Name","name":"ApplicantName"},
      {"type":"file","label":"Applicant Image","name":"ApplicantImage"},
      {"type":"date","label":"Date Of Birth","name":"DOB"},
      {"type":"text","label":"Age (In Years)","name":"Age"},
      {"type":"text","label":"Father/Husband/Guardian","name":"Father_Guardian"},
      {"type":"text","label":"Mother's Name","name":"MotherName","isFormSpecific":true},
      {"type":"date","label":"Date Of Marraiage","name":"DateOfMarriage","isFormSpecific":true},
      {"type":"select","label":"Category","name":"Category","options":["Male","Female","Others"]},
      {"type":"text","label":"Mobile Number","name":"MobileNumber"},
      {"type":"email","label":"Email","name":"Email"},
    ]
  },
  {
    "section": "Present Address Details",
    "fields":[
      {"type":"text","label":"Present Address","name":"PresentAddress"},
      {"type":"select","label":"Present District","name":"PresentDistrict"},
      {"type":"select","label":"Present Tehsil","name":"PresentTehsil"},
      {"type":"select","label":"Present Block","name":"PresentBlock"},
      {"type":"text","label":"Present Halqa Panchayat/ Muncipality Name","name":"PresentPanchayatMuncipality"},
      {"type":"text","label":"Present Village Name","name":"PresentVillage"},
      {"type":"text","label":"Present Ward Name","name":"PresentWard"},
      {"type":"text","label":"Present Pincode","name":"PresentPincode"},
    ]
  },
    {
    "section": "Permanent Address Details",
    "fields":[
      {"type":"text","label":"Permanent Address","name":"PermanentAddress"},
      {"type":"select","label":"Permanent District","name":"PermanentDistrict"},
      {"type":"select","label":"Permanent Tehsil","name":"PermanentTehsil"},
      {"type":"select","label":"Permanent Block","name":"PermanentBlock"},
      {"type":"text","label":"Permanent Halqa Panchayat/ Muncipality Name","name":"PermanentPanchayatMuncipality"},
      {"type":"text","label":"Permanent Village Name","name":"PermanentVillage"},
      {"type":"text","label":"Permanent Ward Name","name":"PermanentWard"},
      {"type":"text","label":"Permanent Pincode","name":"PermanentPincode"},
    ]
  },
  {
    "section":"Bank Details",
    "fields":[
      {"type":"select","label":"Bank Name", "name":"BankName","options":["THE JAMMU AND KASHMIR BANK LTD.","J & K GRAMEEN BANK","ELLAQUAI DEHTI BANK","INDIA POST PAYMENTS BANK"]},
      {"type":"text","label":"Branch Name","name":"BranchName"},
      {"type":"text","label":"IFSC Code","name":"IfscCode"},
      {"type":"text","label":"Account Number","name":"AccountNumber"},
    ]
  }
];
