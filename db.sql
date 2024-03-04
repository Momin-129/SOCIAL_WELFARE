CREATE PROCEDURE InsertCitizenDetails
	@ApplicantName VARCHAR(255),
	@ApplicantImage VARCHAR(255),
	@DOB DATE,
	@Parentage VARCHAR(255),
	@FormSpecific VARCHAR(MAX),
	@Category VARCHAR(100),
	@MobileNumber VARCHAR(10),
	@MailId VARCHAR(255),
	@PresentDistrictId INT,
    @PresentTehsilId INT,
    @PresentBlockId INT,
    @PresentHalqaPanchayatName VARCHAR(255),
    @PresentVillageName VARCHAR(255),
    @PresentWardName VARCHAR(255),
    @PresentPincode INT,
    @PresentAddressDetails VARCHAR(255),
    @SameAsPresent INT,
	@PermanentDistrictId INT,
    @PermanentTehsilId INT,
    @PermanentBlockId INT,
    @PermanentHalqaPanchayatName VARCHAR(255),
    @PermanentVillageName VARCHAR(255),
    @PermanentWardName VARCHAR(255),
    @PermanentPincode INT,
    @PermanentAddressDetails VARCHAR(255),
	@BankName VARCHAR(255),
	@BranchName VARCHAR(255),
	@IFSCCode VARCHAR(20),
	@AccountNumber VARCHAR(100),
	@ServiceId INT
AS
BEGIN
	DECLARE @NewUUID INT;
    DECLARE @Phases VARCHAR(255);
	

    DECLARE @PresentAddressId INT;
    EXEC @PresentAddressId = CheckAndInsertAddress @PresentDistrictId, @PresentTehsilId, @PresentBlockId, @PresentHalqaPanchayatName, @PresentVillageName, @PresentWardName, @PresentPincode, @PresentAddressDetails;

    DECLARE @PermanentAddressId INT;
    IF @SameAsPresent = 1
    BEGIN
        -- If yes, set PermanentAddressId to the same value as PresentAddressId
        SET @PermanentAddressId = @PresentAddressId;
    END
    ELSE
    BEGIN
        -- If no, check and insert Permanent Address
        EXEC @PermanentAddressId = CheckAndInsertAddress @PermanentDistrictId, @PermanentTehsilId, @PermanentBlockId, @PermanentHalqaPanchayatName, @PermanentVillageName, @PermanentWardName, @PermanentPincode, @PermanentAddressDetails;
    END

    -- Check and insert Bank Details
    DECLARE @BankDetailsId INT;
    EXEC @BankDetailsId = InsertBankDetails @BankName, @BranchName, @IFSCCode, @AccountNumber;


	INSERT INTO CitizenDetail (ApplicantName, ApplicantImage,DOB,Father_Guardian,FormSpecific,Category,MobileNumber,MailId,PresentAddressId,PermanentAddressId,BankDetailsId,ServiceId)
	VALUES (@ApplicantName, @ApplicantImage,@DOB,@Parentage,@FormSpecific,@Category,@MobileNumber,@MailId,@PresentAddressId,@PermanentAddressId,@BankDetailsId,@ServiceId);
	
	 -- Get the UUID of the newly inserted record
    SET @NewUUID = (SELECT UUID FROM CitizenDetail WHERE UUID = SCOPE_IDENTITY());

    -- Retrieve the Phase from the Services table based on ServiceId
    SET @Phases = (SELECT Phases FROM ServiceSpecific  WHERE ServiceId = @ServiceId);

    -- Insert into RequestPhase table
    INSERT INTO RequestPhase (CitizenId, ServiceId, Phases,Remarks)
    VALUES (@NewUUID, @ServiceId, @Phases,@Phases);

    -- Return the inserted records
    SELECT * FROM CitizenDetail WHERE UUID = @NewUUID;
END

	