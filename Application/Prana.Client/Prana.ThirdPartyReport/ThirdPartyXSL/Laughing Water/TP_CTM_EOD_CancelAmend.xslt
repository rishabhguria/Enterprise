<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	
	 <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>

	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>


        <AllocID>
			<xsl:value-of select ="'AllocID'"/>
        </AllocID>

        <AllocTransType>
			<xsl:value-of select ="'AllocTransType'"/>
        </AllocTransType>

        <AllocType>
          <xsl:value-of select="'AllocType'"/>
        </AllocType>

        <SecondaryAllocID>
          <xsl:value-of select="'SecondaryAllocID'"/>
        </SecondaryAllocID>

        <RefAllocID>
			<xsl:value-of select ="'RefAllocID'"/>
        </RefAllocID>

        <AllocCancReplaceReason>
          <xsl:value-of select="'AllocCancReplaceReason'"/>
        </AllocCancReplaceReason>

        <BrokerID>
          <xsl:value-of select="'BrokerID'"/>
        </BrokerID>

        <AllocNoOrdersType>
          <xsl:value-of select="'AllocNoOrdersType'"/>
        </AllocNoOrdersType>


        <ClOrdID>
          <xsl:value-of select="'ClOrdID'"/>
        </ClOrdID>

        <Side>
			<xsl:value-of select="'Side'"/>
        </Side>


        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <SymbolSfx>
          <xsl:value-of select="'SymbolSfx'"/>
        </SymbolSfx>

        <SecurityID>
			<xsl:value-of select="'SecurityID'"/>
        </SecurityID>

        <SecurityIDSource>
			<xsl:value-of select="'SecurityIDSource'"/>
        </SecurityIDSource>


        <CFICode>
          <xsl:value-of select="'CFICode'"/>
        </CFICode>

        <SecurityType>
			<xsl:value-of select="'SecurityType'"/>
        </SecurityType>

        <MaturityDate>
          <xsl:value-of select="'MaturityDate'"/>
        </MaturityDate>

        <IssueDate>
          <xsl:value-of select="'IssueDate'"/>
        </IssueDate>

        <Factor>
          <xsl:value-of select="'Factor'"/>
        </Factor>

        <StrikePrice>
			<xsl:value-of select="'StrikePrice'"/>
        </StrikePrice>

        <StrikeCurrency>
          <xsl:value-of select="'StrikeCurrency'"/>
        </StrikeCurrency>

        <StrikeMultiplier>
          <xsl:value-of select="'StrikeMultiplier'"/>
        </StrikeMultiplier>

        <StrikeValue>
          <xsl:value-of select="'StrikeValue'"/>
        </StrikeValue>

        <CountryOfIssue>
          <xsl:value-of select="'CountryOfIssue'"/>
        </CountryOfIssue>

        <ContractMultiplier>
          <xsl:value-of select="'ContractMultiplier'"/>
        </ContractMultiplier>

        <PutOrCall>
			<xsl:value-of select="'PutOrCall'"/>
        </PutOrCall>

        <SecurityExchange>
          <xsl:value-of select="'SecurityExchange'"/>
        </SecurityExchange>

        <CouponRate>
          <xsl:value-of select="'CouponRate'"/>
        </CouponRate>

        <Issuer>
          <xsl:value-of select="'Issuer'"/>
        </Issuer>

        <SecurityDesc>
          <xsl:value-of select="'SecurityDesc'"/>
        </SecurityDesc>

        <InterestAccrualDate>
          <xsl:value-of select="'InterestAccrualDate'"/>
        </InterestAccrualDate>

        <UnderlyingSymbol>
			<xsl:value-of select="'UnderlyingSymbol'"/>
        </UnderlyingSymbol>

        <UnderlyingSecurityID>
          <xsl:value-of select="'UnderlyingSecurityID'"/>
        </UnderlyingSecurityID>

        <UnderlyingSecurityIDSource>
          <xsl:value-of select="'UnderlyingSecurityIDSource'"/>
        </UnderlyingSecurityIDSource>

        <UnderlyingSecurityType>
          <xsl:value-of select="'UnderlyingSecurityType'"/>
        </UnderlyingSecurityType>

        <UnderlyingMaturityMonthYear>
          <xsl:value-of select="'UnderlyingMaturityMonthYear'"/>
        </UnderlyingMaturityMonthYear>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <QtyType>
			<xsl:value-of select="'QtyType'"/>
        </QtyType>

        <PriceType>
          <xsl:value-of select="'PriceType'"/>
        </PriceType>

        <AvgPx>
          <xsl:value-of select="'AvgPx'"/>
        </AvgPx>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>

        <AvgPxPrecision>
          <xsl:value-of select="'AvgPxPrecision'"/>
        </AvgPxPrecision>

        <PartyID>
          <xsl:value-of select="'PartyID'"/>
        </PartyID>

        <PartyIDSource>
          <xsl:value-of select="'PartyIDSource'"/>
        </PartyIDSource>

        <PartyRole>
          <xsl:value-of select="'PartyRole'"/>
        </PartyRole>

        <TradeDate>
          <xsl:value-of select ="'TradeDate'"/>
        </TradeDate>

        <SettlDate>
          <xsl:value-of select ="'SettlDate'"/>
        </SettlDate>

        <BookingType>
          <xsl:value-of select="'BookingType'"/>
        </BookingType>

        <GrossTradeAmt>
          <xsl:value-of select="'GrossTradeAmt'"/>
        </GrossTradeAmt>

        <Concession>
          <xsl:value-of select="'Concession'"/>
        </Concession>

        <NetMoney>
          <xsl:value-of select="'NetMoney'"/>
        </NetMoney>

        <PositionEffect>
			<xsl:value-of select="'PositionEffect'"/>
        </PositionEffect>

        <Text>
          <xsl:value-of select="'Text'"/>
        </Text>

        <NumDaysInterest>
          <xsl:value-of select="'NumDaysInterest'"/>
        </NumDaysInterest>

        <AccruedInterestAmt>
          <xsl:value-of select="'AccruedInterestAmt'"/>
        </AccruedInterestAmt>

        
        <AllocAccount>
			<xsl:value-of select ="'AllocAccount'"/>
        </AllocAccount>

        <AllocAcctIDSource>
          <xsl:value-of select="'AllocAcctIDSource'"/>
        </AllocAcctIDSource>

        <AlocQty>
          <xsl:value-of select="'AllocQty'"/>
        </AlocQty>

        <IndividualAllocID>
          <xsl:value-of select="'IndividualAllocID'"/>
        </IndividualAllocID>

        <ProcessCode>
          <xsl:value-of select="'ProcessCode'"/>
        </ProcessCode>

        <NestedPartyID>
          <xsl:value-of select="'NestedPartyID'"/>
        </NestedPartyID>

        <NestedPartyIDSource>
          <xsl:value-of select="'NestedPartyIDSource'"/>
        </NestedPartyIDSource>

        <NestedPartyRole>
          <xsl:value-of select="'NestedPartyRole'"/>
        </NestedPartyRole>

        <AllocText>
          <xsl:value-of select="'AllocText'"/>
        </AllocText>



        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <CommType>
          <xsl:value-of select="'CommType'"/>
        </CommType>

        <AllocCommissionAmount1>
          <xsl:value-of select="'AllocCommissionAmount 1'"/>
        </AllocCommissionAmount1>

        <AllocCommissionAmountType1>
          <xsl:value-of select="'AllocCommissionAmountType 1'"/>
        </AllocCommissionAmountType1>

        <AllocCommissionAmountSubType1>
          <xsl:value-of select="'AllocCommissionAmountSubType 1'"/>
        </AllocCommissionAmountSubType1>

        <AllocCommissionBasis1>
          <xsl:value-of select="'AllocCommissionBasis 1'"/>
        </AllocCommissionBasis1>

        <AllocAvgPx>
          <xsl:value-of select="'AllocAvgPx'"/>
        </AllocAvgPx>

        <AllocNetMoney>
          <xsl:value-of select="'AllocNetMoney'"/>
        </AllocNetMoney>

        <AllocSettlCurrAmt>
          <xsl:value-of select="'AllocSettlCurrAmt'"/>
        </AllocSettlCurrAmt>

        <AllocSettlCurrency>
          <xsl:value-of select="'AllocSettlCurrency'"/>
        </AllocSettlCurrency>

        <SettlCurrFxRate>
          <xsl:value-of select="'SettlCurrFxRate'"/>
        </SettlCurrFxRate>

        <SettlCurrFxRateCalc>
          <xsl:value-of select="'SettlCurrFxRateCalc'"/>
        </SettlCurrFxRateCalc>

        <AllocAccruedInterestAmt>
          <xsl:value-of select="'AllocAccruedInterestAmt'"/>
        </AllocAccruedInterestAmt>

        <MiscFeeAmt>
          <xsl:value-of select="'MiscFeeAmt'"/>
        </MiscFeeAmt>

        <MiscFeeCurr>
          <xsl:value-of select="'MiscFeeCurr'"/>
        </MiscFeeCurr>

        <MiscFeeType>
          <xsl:value-of select="'MiscFeeType'"/>
        </MiscFeeType>

        <MiscFeeBasis>
          <xsl:value-of select="'MiscFeeBasis'"/>
        </MiscFeeBasis>


        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail[TaxLotState!='Sent']">
				<xsl:choose>
					
					<xsl:when test ="TaxLotState!='Amemded'">
				       <ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxLotState>

					
			   <AllocID>
                <xsl:choose>
					
					<xsl:when test ="TaxLotState = 'Allocated'">
                   <!--<xsl:value-of  select="PBUniqueID"/>-->
						<xsl:value-of  select="PBUniqueID"/>
                 </xsl:when>
                 <xsl:when test ="TaxLotState = 'Deleted'">
                   <xsl:value-of select ="concat(PBUniqueID,'C')"/>
                 </xsl:when>
                  </xsl:choose>
		        </AllocID>		

		        <AllocTransType>
                 <xsl:choose>
                 <xsl:when test ="TaxLotState = 'Allocated'">
                   <xsl:value-of  select="'0'"/>
                  </xsl:when>
                  <xsl:when test ="TaxLotState = 'Deleted'">
                      <xsl:value-of select ="'2'"/>
                   </xsl:when>
                 </xsl:choose>
		      </AllocTransType>

					<AllocType>
						<xsl:value-of select="''"/>
					</AllocType>

					<SecondaryAllocID>
						<xsl:value-of select="''"/>
					</SecondaryAllocID>
				
		    <RefAllocID>
            <xsl:choose>
              <xsl:when test ="TaxLotState = 'Allocated'">
                <xsl:value-of  select="''"/>
              </xsl:when>
              <xsl:when test ="TaxLotState = 'Deleted'">
                <!--<xsl:value-of select ="GroupID"/>-->
			  <xsl:value-of  select="PBUniqueID"/>
              </xsl:when>
            </xsl:choose>
					</RefAllocID>

				<AllocCancReplaceReason>
				 <xsl:choose>
                 <xsl:when test ="TaxLotState = 'Allocated'">
                   <xsl:value-of  select="''"/>
                  </xsl:when> 
               <xsl:when test ="TaxLotState = 'Deleted'">
                <xsl:value-of select ="'1'"/>
              </xsl:when>				  
                 </xsl:choose>						
				</AllocCancReplaceReason>
					
         <xsl:variable name="PB_NAME">
            <xsl:value-of select="'Test_Constellation'"/>
          </xsl:variable>
					
          <xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

          <xsl:variable name="THIRDPARTY_BROKER_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@ThirdPartyBroker=$PRANA_BROKER_NAME]/@PranaBroker"/>
          </xsl:variable>

          <xsl:variable name="varCounterParty">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
                <xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_BROKER_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

					<BrokerID>
						<xsl:value-of select="$varCounterParty"/>
					</BrokerID>

					<AllocNoOrdersType >
						<xsl:value-of select="''"/>
					</AllocNoOrdersType >


					<ClOrdID>
						<xsl:value-of select="''"/>
					</ClOrdID>
					
			 <Side>
            <xsl:choose>
              <xsl:when test ="Side = 'Buy' or Side = 'Buy to Open' or Side = 'Buy to Close'">
                <xsl:value-of  select="'1'"/>
              </xsl:when>
              <xsl:when test ="Side = 'Sell' or Side = 'Sell to Close'">
                <xsl:value-of  select="'2'"/>
              </xsl:when>
              <xsl:when test ="Side = 'Sell short' or Side = 'Sell to Open'">
                <xsl:value-of  select="'5'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
			</Side>
					
					

					<Symbol>
						<xsl:choose>
							<xsl:when test="Asset = 'FixedIncome'">
								<xsl:value-of select="UnderlyingSymbol"/>
							</xsl:when>							
						
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

          
					<SymbolSfx>
						<xsl:value-of select="''"/>
					</SymbolSfx>
					
			<SecurityID>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and CurrencySymbol ='USD' and CUSIP !=''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:when test="Asset='Equity' and CurrencySymbol !='USD' and SEDOL !=''">
                <xsl:value-of select ="SEDOL"/>
              </xsl:when>             
              <xsl:when test ="Asset = 'EquityOption' and OSIOptionSymbol !=''">
                <xsl:value-of select ="OSIOptionSymbol"/>
              </xsl:when>
			  <xsl:when test ="Asset = 'FixedIncome' and CUSIP !=''">
                <xsl:value-of select="CUSIP"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityID>
				
		      <SecurityIDSource>
            <xsl:choose>
				<xsl:when test="(Asset='Equity' or Asset='FixedIncome') and CurrencySymbol ='USD' and CUSIP !=''">
					<xsl:value-of select="'1'"/>
				</xsl:when>
              <xsl:when test="Asset='Equity' and CurrencySymbol !='USD' and SEDOL !=''">
                <xsl:value-of select ="'2'"/>
              </xsl:when>
              <xsl:when test ="Asset = 'EquityOption' and OSIOptionSymbol !=''">
                <xsl:value-of select ="'H'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
		 </SecurityIDSource>


		 <xsl:variable name="varCFICode">
			 <xsl:choose>             
              <xsl:when test ="Asset = 'EquityOption'">
              <xsl:choose>
							<xsl:when test="PutOrCall='PUT'">
								<xsl:value-of select="'OPXXCS'"/>							
							</xsl:when>
						  <xsl:when test="PutOrCall='CALL'">
								<xsl:value-of select="'OCXXCS'"/>							
							</xsl:when>
						</xsl:choose>	
              </xsl:when>
			<xsl:when test ="Asset = 'Future'">				
			  <xsl:value-of select="'FXXXXX'"/>
			</xsl:when>	 					 
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
		</xsl:variable>
					<CFICode>
						<xsl:value-of select="$varCFICode"/>
					</CFICode>
          
					<SecurityType>
            <xsl:choose>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="'CS'"/>
              </xsl:when>
              <xsl:when test="Asset = 'FixedIncome'">
                <xsl:value-of select="'CORP'"/>
              </xsl:when>
              <xsl:when test="Asset = 'Future'">
                <xsl:value-of select="'FUT'"/>
              </xsl:when>
              <xsl:when test ="Asset = 'EquityOption'">
                <xsl:value-of select ="'OPT'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityType>
						   
			 <xsl:variable name="varExpirationDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="ExpirationDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
             
					
		  <xsl:variable name="EXP_Day" select="substring-before(substring-after($varExpirationDate,'/'),'/')"/>
          <xsl:variable name="EXP_Month" select="substring-before($varExpirationDate,'/')"/>
          <xsl:variable name="EXP_Year" select="substring-after(substring-after($varExpirationDate,'/'),'/')"/>

            <xsl:variable name="varMaturityDate">
						<xsl:choose>
							<xsl:when test ="Asset = 'EquityOption' or Asset='Future' or Asset ='FixedIncome'">
								 <xsl:value-of select="concat($EXP_Year,$EXP_Month,$EXP_Day)"/>
							</xsl:when>						
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>					
					</xsl:variable>

					<MaturityDate>
						<xsl:value-of select="$varMaturityDate"/>
					</MaturityDate>
						   
			  <xsl:variable name="varIssueDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="IssueDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
					
		  <xsl:variable name="Issue_Day" select="substring-before(substring-after($varIssueDate,'/'),'/')"/>
          <xsl:variable name="Issue_Month" select="substring-before($varIssueDate,'/')"/>
          <xsl:variable name="Issue_Year" select="substring-after(substring-after($varIssueDate,'/'),'/')"/>

	     <xsl:variable name="varFixdDate">
						<xsl:choose>
							<xsl:when test ="Asset ='FixedIncome'">
								 <xsl:value-of select="concat($Issue_Year,$Issue_Month,$Issue_Day)"/>
							</xsl:when>						
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>					
					</xsl:variable>
					<IssueDate>
						<xsl:value-of select="$varFixdDate"/>
					</IssueDate>

					<Factor>
						<xsl:choose>
							<xsl:when test ="Asset ='FixedIncome'">
								 <xsl:value-of select="''"/>
							</xsl:when>						
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>	
					</Factor>
          
		<StrikePrice>
            <xsl:choose>
              <xsl:when test ="Asset = 'EquityOption'">
                <xsl:value-of select="StrikePrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </StrikePrice>

					<StrikeCurrency>
						<xsl:value-of select="''"/>
					</StrikeCurrency>

					<StrikeMultiplier>
						<xsl:value-of select="''"/>
					</StrikeMultiplier>

					<StrikeValue>
						<xsl:value-of select="''"/>
					</StrikeValue>
					
					
			<xsl:variable name="PRANA_COUNTRY_NAME" select="CountryName"/>
			
          <xsl:variable name="THIRDPARTY_COUNTRY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_CountryCodeMapping.xml')/CountryCodeMapping/PB[@Name=$PB_NAME]/CountryData[@PranaCountryCode=$PRANA_COUNTRY_NAME]/@PBCountryName"/>
          </xsl:variable>

          <xsl:variable name="varCountryOfIssue">
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_COUNTRY_NAME != ''">
                <xsl:value-of select="$THIRDPARTY_COUNTRY_NAME"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_COUNTRY_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


					<CountryOfIssue>
						<xsl:value-of select="$varCountryOfIssue"/>
					</CountryOfIssue>
				

					<ContractMultiplier>					
						<xsl:choose>
							
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="AssetMultiplier"/>
							</xsl:when>
							<xsl:when test="Asset='FixedIncome'">								
								<xsl:value-of select="1"/>
							</xsl:when>	
							<xsl:when test="AssetMultiplier !='1'">								
								<xsl:value-of select="AssetMultiplier"/>
							</xsl:when>	
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</ContractMultiplier>
					
					<xsl:variable name="varPutOrCall">
						<xsl:choose>
							<xsl:when test ="Asset = 'EquityOption'">
								 <xsl:value-of select="PutOrCall"/>
							</xsl:when>						
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>					
					</xsl:variable>
					<PutOrCall>
						<xsl:choose>
							<xsl:when test="$varPutOrCall='PUT'">
								<xsl:value-of select="'0'"/>							
							</xsl:when>
						  <xsl:when test="$varPutOrCall='CALL'">
								<xsl:value-of select="'1'"/>							
							</xsl:when>
						</xsl:choose>						
					</PutOrCall>

					<xsl:variable name="PRANA_MIC_NAME" select="Exchange"/>
					<xsl:variable name="varCCY" select="CurrencySymbol"/>

					<xsl:variable name="THIRDPARTY_MIC_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdPartyMIC_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@Exchange=$PRANA_MIC_NAME]/@PranaMICCode"/>
					</xsl:variable>

					<xsl:variable name="varSecurityExchange">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_MIC_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_MIC_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<SecurityExchange>
						<xsl:value-of select="$varSecurityExchange"/>
					</SecurityExchange>

					<CouponRate>
						<xsl:choose>
							<xsl:when test ="Asset ='FixedIncome'">
								 <xsl:value-of select="Coupon"/>
							</xsl:when>						
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>						
					</CouponRate>

					<Issuer>
						<xsl:value-of select="''"/>						
					</Issuer>
						 

					<SecurityDesc>
						<xsl:value-of select="CompanyName"/>
					</SecurityDesc>

					<InterestAccrualDate>
						<xsl:value-of select="''"/>
					</InterestAccrualDate>
          
			<UnderlyingSymbol>
            <xsl:choose>
              <xsl:when test ="Asset = 'EquityOption'">
                <xsl:value-of select="UnderlyingSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </UnderlyingSymbol>

					<UnderlyingSecurityID>
						<xsl:value-of select="''"/>
					</UnderlyingSecurityID>

					<UnderlyingSecurityIDSource>
						<xsl:value-of select="''"/>
					</UnderlyingSecurityIDSource>

					<UnderlyingSecurityType>
						<xsl:value-of select="''"/>
					</UnderlyingSecurityType>

					<UnderlyingMaturityMonthYear>
						<xsl:value-of select="''"/>
					</UnderlyingMaturityMonthYear>

					<Quantity>
						<xsl:value-of select="CumQty"/>
					</Quantity>

					<QtyType>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
								<xsl:value-of select="'1'"/>							
							</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'0'"/>						
						</xsl:otherwise>
						</xsl:choose>						
					</QtyType>

					<PriceType>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">								
								<xsl:value-of select="1"/>
							</xsl:when>	
						<xsl:otherwise>
							<xsl:value-of select="2"/>
						</xsl:otherwise>
						</xsl:choose>						
					</PriceType>

					<AvgPx>
						<xsl:value-of select="AvgPrice"/>
					</AvgPx>

					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<AvgPxPrecision>
						<xsl:value-of select="''"/>
					</AvgPxPrecision>
					
					<xsl:variable name="THIRDPARTY_PartyID_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@PartyID"/>
					</xsl:variable>

					<xsl:variable name="varPartyID">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_PartyID_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_PartyID_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<PartyID>
						<xsl:value-of select="$varPartyID"/>
					</PartyID>

					<xsl:variable name="THIRDPARTY_PartyIDSource_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@PartyIDSource"/>
					</xsl:variable>
					
					<xsl:variable name="varPartyIDSource">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_PartyIDSource_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_PartyIDSource_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<PartyIDSource>
						<xsl:value-of select="$varPartyIDSource"/>
					</PartyIDSource>

					<xsl:variable name="THIRDPARTY_PartyRole_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@PartyRole"/>
					</xsl:variable>
					
					<xsl:variable name="varPartyRole">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_PartyRole_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_PartyRole_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<PartyRole>
						<xsl:value-of select="$varPartyRole"/>
					</PartyRole>
						   
			   <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
              </TradeDate>
						   
			   <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlDate>
                <xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
              </SettlDate>

				

					<BookingType>
							<xsl:choose>
							<xsl:when test="(Asset='Equity' and IsSwapped='true')">
								<xsl:value-of select="'1'"/>							
							</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'0'"/>					
						</xsl:otherwise>
					</xsl:choose>                       
                   </BookingType>

					<xsl:variable name="varGrossTradeAmt">
						<xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier)"/>
					</xsl:variable>
					
					<GrossTradeAmt>
						<xsl:value-of select="format-number($varGrossTradeAmt,'0.####')"/>
					</GrossTradeAmt>

					<Concession>
						<xsl:value-of select="''"/>
					</Concession>
					
					<xsl:variable name="varAccruedInterest">
						<xsl:value-of select="format-number(AccruedInterest_BlockLevel,'0.####')"/>
					</xsl:variable>

          <xsl:variable name="varAlloNetamount">
            <xsl:choose>
              <xsl:when test="contains(Side,'Buy')">
                <xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier) + Commission_BlockLevel + SoftCommission_BlockLevel + OtherBrokerFees_BlockLevel + ClearingBrokerFee_BlockLevel + StampDuty_BlockLevel + TransactionLevy_BlockLevel + ClearingFee_BlockLevel + TaxOnCommissions_BlockLevel + MiscFees_BlockLevel + SecFee_BlockLevel + OccFee_BlockLevel + OrfFee_BlockLevel"/>
              </xsl:when>
              <xsl:when test="contains(Side,'Sell')">
                <xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier) - (Commission_BlockLevel + SoftCommission_BlockLevel + OtherBrokerFees_BlockLevel + ClearingBrokerFee_BlockLevel + StampDuty_BlockLevel + TransactionLevy_BlockLevel + ClearingFee_BlockLevel + TaxOnCommissions_BlockLevel + MiscFees_BlockLevel + SecFee_BlockLevel + OccFee_BlockLevel + OrfFee_BlockLevel)"/>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>
          
                    <NetMoney>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="format-number(($varAlloNetamount + $varAccruedInterest),'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($varAlloNetamount,'0.####')"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetMoney>
					
					<xsl:variable name="varPositionEffect">
						<xsl:choose>
							<xsl:when test="(Side = 'Buy' or Side = 'Sell short' or Side='Buy to Open' or Side = 'Sell to Open')">
								<xsl:value-of select="'O'"/>							
							</xsl:when>
							
						<xsl:when test="(Side = 'Sell' or Side = 'Buy to Close' or Side='Sell to Close')">
								<xsl:value-of select="'C'"/>							
							</xsl:when>						
						</xsl:choose>					
					</xsl:variable>

					<PositionEffect>
						<xsl:choose>
							<xsl:when test="(Asset='Equity' and IsSwapped='true') or Asset='EquityOption' ">
								<xsl:value-of select="$varPositionEffect"/>							
							</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="''"/>						
						</xsl:otherwise>
					</xsl:choose>
					</PositionEffect>

					<Text>
						<xsl:value-of select="''"/>
					</Text>

					<NumDaysInterest>
						<xsl:value-of select="''"/>
					</NumDaysInterest>

					<AccruedInterestAmt>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">								
								<xsl:value-of select="$varAccruedInterest"/>
							</xsl:when>	
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
						</xsl:choose>	
					</AccruedInterestAmt>
					
		     <xsl:variable name = "PRANA_FUND_NAME">
                <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>
  					<AllocAccount>
			         <xsl:choose>               
                         <xsl:when test ="$THIRDPARTY_FUND_CODE != ''">
                      <xsl:value-of select ="$THIRDPARTY_FUND_CODE"/>
                     </xsl:when>
                      <xsl:otherwise>
                         <xsl:value-of select ="$PRANA_FUND_NAME"/>
                       </xsl:otherwise>
                     </xsl:choose>
				</AllocAccount>

					<AllocAcctIDSource>
						<xsl:value-of select="''"/>
					</AllocAcctIDSource>

					<AlocQty>
						<xsl:value-of select="OrderQty"/>
					</AlocQty>

					<IndividualAllocID>
						<xsl:value-of select="''"/>
					</IndividualAllocID>
					
                <xsl:variable name="THIRDPARTY_ProcessCode_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@ProcessCode"/>
					</xsl:variable>
					
					<xsl:variable name="varProcessCode">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_ProcessCode_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_ProcessCode_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<ProcessCode>
						<xsl:choose>
							<xsl:when test="(Asset='Equity' and IsSwapped='true')">
								<xsl:value-of select="$varProcessCode"/>						
							</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'0'"/>					
						</xsl:otherwise>
					</xsl:choose>						
					</ProcessCode>
					
					
					<xsl:variable name="THIRDPARTY_NestedPartyID_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@NestedPartyID"/>
					</xsl:variable>
					
					<xsl:variable name="varNestedPartyID">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_NestedPartyID_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_NestedPartyID_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<NestedPartyID>
					<xsl:choose>
							<xsl:when test="(Asset='Equity' and IsSwapped='true') or Asset='EquityOption'">
								<xsl:value-of select="$varCounterParty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varNestedPartyID"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</NestedPartyID>
					
					<xsl:variable name="THIRDPARTY_NestedPartyIDSource_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@NestedPartyIDSource"/>
					</xsl:variable>
					
					<xsl:variable name="varNestedPartyIDSource">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_NestedPartyIDSource_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_NestedPartyIDSource_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<NestedPartyIDSource>
					<xsl:choose>
							<xsl:when test="(Asset='Equity' and IsSwapped !='true') or Asset='FixedIncome' ">
								<xsl:value-of select="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$varNestedPartyIDSource"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</NestedPartyIDSource>
					
					<xsl:variable name="THIRDPARTY_NestedPartyRole_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@NestedPartyRole"/>
					</xsl:variable>
					
					<xsl:variable name="varNestedPartyRole">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_NestedPartyRole_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_NestedPartyRole_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<NestedPartyRole>
					<xsl:choose>
							<xsl:when test="(Asset='Equity' and IsSwapped !='true') or Asset='FixedIncome' ">
								<xsl:value-of select="''"/>							
							</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$varNestedPartyRole"/>					
						</xsl:otherwise>
					</xsl:choose>
						
					</NestedPartyRole>

					<AllocText>
						<xsl:value-of select="''"/>
					</AllocText>

					<xsl:variable name="varCommission">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>
					<Commission>
						<xsl:value-of select="format-number(CommissionCharged,'0.####')"/>
					</Commission>

					<CommType>
						<xsl:value-of select="'3'"/>
					</CommType>

					<AllocCommissionAmount1>
						<xsl:value-of select="format-number(CommissionCharged,'0.####')"/>
					</AllocCommissionAmount1>

					<AllocCommissionAmountType1>
						<xsl:value-of select="'0'"/>
					</AllocCommissionAmountType1>

					<AllocCommissionAmountSubType1>
						<xsl:value-of select="'2'"/>
					</AllocCommissionAmountSubType1>

					<AllocCommissionBasis1>
						<xsl:value-of select="'3'"/>
					</AllocCommissionBasis1>

                   

					<AllocAvgPx>
						<xsl:value-of select="AvgPrice"/>
					</AllocAvgPx>
					
		<xsl:variable name="varAllocNetamount">
            <xsl:choose>
              <xsl:when test="contains(Side,'Buy')">
                <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
              </xsl:when>
              <xsl:when test="contains(Side,'Sell')">
                <xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

					
					<AllocNetMoney>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="format-number(($varAllocNetamount + AccruedInterest),'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($varAllocNetamount,'0.####')"/>
							</xsl:otherwise>
						</xsl:choose>						
					</AllocNetMoney>

	
					
		  <xsl:variable name="varFXRate">
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
          <xsl:variable name ="varNetAmount">
            <xsl:choose>
              <xsl:when test="$varFXRate=0">
                <xsl:value-of select="$varAllocNetamount"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='M'">
                <xsl:value-of select="$varAllocNetamount * $varFXRate"/>
              </xsl:when>

              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='D'">
                <xsl:value-of select="$varAllocNetamount div $varFXRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

					<AllocSettlCurrAmt>	
                       <xsl:choose>
							<xsl:when test="Asset='FixedIncome'">
								<xsl:value-of select="format-number(($varNetAmount + AccruedInterest),'0.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number($varNetAmount,'0.####')"/>
							</xsl:otherwise>
						</xsl:choose>		
					</AllocSettlCurrAmt>

					<AllocSettlCurrency>
						<xsl:value-of select="SettlCurrency"/>
					</AllocSettlCurrency>

					<SettlCurrFxRate>
			  		 <xsl:choose>
                       <xsl:when test="SettlCurrency = CurrencySymbol">
                          <xsl:value-of select="1"/>
                       </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="FXRate_Taxlot"/>	
                           </xsl:otherwise>
                        </xsl:choose> 									
					</SettlCurrFxRate>

					<SettlCurrFxRateCalc>
						<xsl:value-of select="FXConversionMethodOperator"/>
					</SettlCurrFxRateCalc>

					<AllocAccruedInterestAmt>
						<xsl:choose>
							<xsl:when test="Asset='FixedIncome'">								
								<xsl:value-of select="format-number(AccruedInterest,'0.####')"/>
							</xsl:when>	
						<xsl:otherwise>
							<xsl:value-of select="''"/>
						</xsl:otherwise>
						</xsl:choose>						
					</AllocAccruedInterestAmt>

					<MiscFeeAmt>
						<xsl:value-of select="format-number((OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee),'0.####')"/>
					</MiscFeeAmt>

					<MiscFeeCurr>
						<xsl:value-of select="CurrencySymbol"/>
					</MiscFeeCurr>

					<MiscFeeType>
						<xsl:value-of select="'7'"/>
					</MiscFeeType>

					<MiscFeeBasis>
						<xsl:value-of select="0"/>
					</MiscFeeBasis>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
					</xsl:when>

					<xsl:otherwise>
						 <xsl:if test ="number(OldExecutedQuantity)">
						<!--<xsl:if test ="TaxLotState ='Amemded'">-->
							<ThirdPartyFlatFileDetail>

								<RowHeader>
									<xsl:value-of select ="'false'"/>
								</RowHeader>

								<TaxLotState>
									<xsl:value-of select="'Deleted'"/>
								</TaxLotState>


								<AllocID>
								 <xsl:value-of select ="concat(PBUniqueID,'C')"/>							
								</AllocID>

								<AllocTransType>								
								
								  <xsl:value-of select ="'2'"/>
								</AllocTransType>

								<AllocType>
									<xsl:value-of select="''"/>
								</AllocType>

								<SecondaryAllocID>
									<xsl:value-of select="''"/>
								</SecondaryAllocID>

								<RefAllocID>																
								<xsl:value-of select ="PBUniqueID"/>
								</RefAllocID>

								<AllocCancReplaceReason>
									<xsl:value-of select="'1'"/>
								</AllocCancReplaceReason>

								<xsl:variable name="PB_NAME">
									<xsl:value-of select="'Test_Constellation'"/>
								</xsl:variable>

								<xsl:variable name="PRANA_BROKER_NAME" select="OldCounterparty"/>

								<xsl:variable name="THIRDPARTY_BROKER_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@ThirdPartyBroker=$PRANA_BROKER_NAME]/@PranaBroker"/>
								</xsl:variable>

								<xsl:variable name="varCounterParty">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
											<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_BROKER_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<BrokerID>
									<xsl:value-of select="$varCounterParty"/>
								</BrokerID>

								<AllocNoOrdersType >
									<xsl:value-of select="''"/>
								</AllocNoOrdersType >


								<ClOrdID>
									<xsl:value-of select="''"/>
								</ClOrdID>

								<Side>
									<xsl:choose>
										<xsl:when test ="OldSide = 'Buy' or OldSide = 'Buy to Open' or OldSide = 'Buy to Close'">
											<xsl:value-of  select="'1'"/>
										</xsl:when>
										<xsl:when test ="OldSide = 'Sell' or OldSide = 'Sell to Close'">
											<xsl:value-of  select="'2'"/>
										</xsl:when>
										<xsl:when test ="OldSide = 'Sell short' or OldSide = 'Sell to Open'">
											<xsl:value-of  select="'5'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</Side>



								<Symbol>
									<xsl:choose>
										<xsl:when test="Asset = 'FixedIncome'">
											<xsl:value-of select="UnderlyingSymbol"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="Symbol"/>
										</xsl:otherwise>
									</xsl:choose>
								</Symbol>

								

								<SymbolSfx>
									<xsl:value-of select="''"/>
								</SymbolSfx>

								<SecurityID>
									<xsl:choose>
										<xsl:when test="Asset='Equity' and CurrencySymbol ='USD' and CUSIP !=''">
											<xsl:value-of select="CUSIP"/>
										</xsl:when>
										<xsl:when test="Asset='Equity' and CurrencySymbol !='USD' and SEDOL !=''">
											<xsl:value-of select ="SEDOL"/>
										</xsl:when>
										<xsl:when test ="Asset = 'EquityOption' and OSIOptionSymbol !=''">
											<xsl:value-of select ="OSIOptionSymbol"/>
										</xsl:when>
										<xsl:when test ="Asset = 'FixedIncome' and CUSIP !=''">
											<xsl:value-of select="CUSIP"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="Symbol"/>
										</xsl:otherwise>
									</xsl:choose>
								</SecurityID>

								<SecurityIDSource>
									<xsl:choose>
										<xsl:when test="(Asset='Equity' or Asset='FixedIncome') and CurrencySymbol ='USD' and CUSIP !=''">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="Asset='Equity' and CurrencySymbol !='USD' and SEDOL !=''">
											<xsl:value-of select ="'2'"/>
										</xsl:when>
										<xsl:when test ="Asset = 'EquityOption' and OSIOptionSymbol !=''">
											<xsl:value-of select ="'H'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SecurityIDSource>


								<xsl:variable name="varCFICode">
									<xsl:choose>
										<xsl:when test ="Asset = 'EquityOption'">
											<xsl:choose>
												<xsl:when test="PutOrCall='PUT'">
													<xsl:value-of select="'OPXXCS'"/>
												</xsl:when>
												<xsl:when test="PutOrCall='CALL'">
													<xsl:value-of select="'OCXXCS'"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:when test ="Asset = 'Future'">
											<xsl:value-of select="'FXXXXX'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<CFICode>
									<xsl:value-of select="$varCFICode"/>
								</CFICode>

								<SecurityType>
									<xsl:choose>
										<xsl:when test="Asset = 'Equity'">
											<xsl:value-of select="'CS'"/>
										</xsl:when>
										<xsl:when test="Asset = 'FixedIncome'">
											<xsl:value-of select="'CORP'"/>
										</xsl:when>
										<xsl:when test="Asset = 'Future'">
											<xsl:value-of select="'FUT'"/>
										</xsl:when>
										<xsl:when test ="Asset = 'EquityOption'">
											<xsl:value-of select ="'OPT'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SecurityType>
								
				  <xsl:variable name="varExpirationDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="ExpirationDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

								<xsl:variable name="EXP_Day" select="substring-before(substring-after($varExpirationDate,'/'),'/')"/>
								<xsl:variable name="EXP_Month" select="substring-before($varExpirationDate,'/')"/>
								<xsl:variable name="EXP_Year" select="substring-after(substring-after($varExpirationDate,'/'),'/')"/>

								<xsl:variable name="varMaturityDate">
									<xsl:choose>
										<xsl:when test ="Asset = 'EquityOption' or Asset='Future' or Asset ='FixedIncome'">
											<xsl:value-of select="concat($EXP_Year,$EXP_Month,$EXP_Day)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<MaturityDate>
									<xsl:value-of select="$varMaturityDate"/>
								</MaturityDate>
								
			 <xsl:variable name="IssueDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="IssueDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

								<xsl:variable name="Issue_Day" select="substring-before(substring-after($IssueDate,'/'),'/')"/>
								<xsl:variable name="Issue_Month" select="substring-before($IssueDate,'/')"/>
								<xsl:variable name="Issue_Year" select="substring-after(substring-after($IssueDate,'/'),'/')"/>

								<xsl:variable name="varIssueDate">
									<xsl:choose>
										<xsl:when test ="Asset ='FixedIncome'">
											<xsl:value-of select="concat($Issue_Year,$Issue_Month,$Issue_Day)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<IssueDate>
									<xsl:value-of select="$varIssueDate"/>
								</IssueDate>

								<Factor>
									<xsl:choose>
										<xsl:when test ="Asset ='FixedIncome'">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</Factor>

								<StrikePrice>
									<xsl:choose>
										<xsl:when test ="Asset = 'EquityOption'">
											<xsl:value-of select="StrikePrice"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</StrikePrice>

								<StrikeCurrency>
									<xsl:value-of select="''"/>
								</StrikeCurrency>

								<StrikeMultiplier>
									<xsl:value-of select="''"/>
								</StrikeMultiplier>

								<StrikeValue>
									<xsl:value-of select="''"/>
								</StrikeValue>


								<xsl:variable name="PRANA_COUNTRY_NAME" select="CountryName"/>

								<xsl:variable name="THIRDPARTY_COUNTRY_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_CountryCodeMapping.xml')/CountryCodeMapping/PB[@Name=$PB_NAME]/CountryData[@PranaCountryCode=$PRANA_COUNTRY_NAME]/@PBCountryName"/>
								</xsl:variable>

								<xsl:variable name="varCountryOfIssue">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_COUNTRY_NAME != ''">
											<xsl:value-of select="$THIRDPARTY_COUNTRY_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$PRANA_COUNTRY_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>


								<CountryOfIssue>
									<xsl:value-of select="$varCountryOfIssue"/>
								</CountryOfIssue>


								<ContractMultiplier>
									<xsl:choose>

										<xsl:when test="Asset='EquityOption'">
											<xsl:value-of select="AssetMultiplier"/>
										</xsl:when>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="1"/>
										</xsl:when>
										<xsl:when test="AssetMultiplier !='1'">
											<xsl:value-of select="AssetMultiplier"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</ContractMultiplier>

								<xsl:variable name="varPutOrCall">
									<xsl:choose>
										<xsl:when test ="Asset = 'EquityOption'">
											<xsl:value-of select="PutOrCall"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<PutOrCall>
									<xsl:choose>
										<xsl:when test="$varPutOrCall='PUT'">
											<xsl:value-of select="'0'"/>
										</xsl:when>
										<xsl:when test="$varPutOrCall='CALL'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
									</xsl:choose>
								</PutOrCall>

								<xsl:variable name="PRANA_MIC_NAME" select="Exchange"/>
								<xsl:variable name="varCCY" select="CurrencySymbol"/>

								<xsl:variable name="THIRDPARTY_MIC_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdPartyMIC_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@Exchange=$PRANA_MIC_NAME]/@PranaMICCode"/>
								</xsl:variable>

								<xsl:variable name="varSecurityExchange">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_MIC_NAME!=''">
											<xsl:value-of select="$THIRDPARTY_MIC_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<SecurityExchange>
									<xsl:value-of select="$varSecurityExchange"/>
								</SecurityExchange>

								<CouponRate>
									<xsl:choose>
										<xsl:when test ="Asset ='FixedIncome'">
											<xsl:value-of select="Coupon"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</CouponRate>

								<Issuer>
									<xsl:value-of select="''"/>
								</Issuer>

								<SecurityDesc>
									<xsl:value-of select="CompanyName"/>
								</SecurityDesc>

								<InterestAccrualDate>
									<xsl:value-of select="''"/>
								</InterestAccrualDate>

								<UnderlyingSymbol>
									<xsl:choose>
										<xsl:when test ="Asset = 'EquityOption'">
											<xsl:value-of select="UnderlyingSymbol"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</UnderlyingSymbol>

								<UnderlyingSecurityID>
									<xsl:value-of select="''"/>
								</UnderlyingSecurityID>

								<UnderlyingSecurityIDSource>
									<xsl:value-of select="''"/>
								</UnderlyingSecurityIDSource>

								<UnderlyingSecurityType>
									<xsl:value-of select="''"/>
								</UnderlyingSecurityType>

								<UnderlyingMaturityMonthYear>
									<xsl:value-of select="''"/>
								</UnderlyingMaturityMonthYear>

								<Quantity>
									<!-- <xsl:value-of select="CumQty"/> -->
									<xsl:value-of select="OldExecutedQuantity_BlockLevel"/>
									
								</Quantity>

								<QtyType>
									<xsl:choose>
										<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>
									</xsl:choose>
								</QtyType>

								<PriceType>
									<xsl:choose>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="1"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="2"/>
										</xsl:otherwise>
									</xsl:choose>
								</PriceType>

								<AvgPx>
									<xsl:value-of select="OldAvgPrice"/>
								</AvgPx>

								<Currency>
									<xsl:value-of select="CurrencySymbol"/>
								</Currency>

								<AvgPxPrecision>
									<xsl:value-of select="''"/>
								</AvgPxPrecision>

								<xsl:variable name="THIRDPARTY_PartyID_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@PartyID"/>
								</xsl:variable>

								<xsl:variable name="varPartyID">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_PartyID_NAME!=''">
											<xsl:value-of select="$THIRDPARTY_PartyID_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<PartyID>
									<xsl:value-of select="$varPartyID"/>
								</PartyID>

								<xsl:variable name="THIRDPARTY_PartyIDSource_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@PartyIDSource"/>
								</xsl:variable>

								<xsl:variable name="varPartyIDSource">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_PartyIDSource_NAME!=''">
											<xsl:value-of select="$THIRDPARTY_PartyIDSource_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<PartyIDSource>
									<xsl:value-of select="$varPartyIDSource"/>
								</PartyIDSource>

								<xsl:variable name="THIRDPARTY_PartyRole_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@PartyRole"/>
								</xsl:variable>

								<xsl:variable name="varPartyRole">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_PartyRole_NAME!=''">
											<xsl:value-of select="$THIRDPARTY_PartyRole_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<PartyRole>
									<xsl:value-of select="$varPartyRole"/>
								</PartyRole>

				<xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="OldTradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
              </TradeDate>
						   
			   <xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="OldSettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <SettlDate>
                <xsl:value-of select="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
              </SettlDate>

								<BookingType>
									<xsl:choose>
										<xsl:when test="(Asset='Equity' and IsSwapped='true')">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>
									</xsl:choose>
								</BookingType>

								<xsl:variable name="varGrossTradeAmt">
									<xsl:value-of select="(OldExecutedQuantity_BlockLevel * OldAvgPrice * AssetMultiplier)"/>
								</xsl:variable>

								<GrossTradeAmt>
									<xsl:value-of select="format-number($varGrossTradeAmt,'0.####')"/>
								</GrossTradeAmt>

								<Concession>
									<xsl:value-of select="''"/>
								</Concession>

								<xsl:variable name="varAccruedInterest">
									<xsl:value-of select="format-number(OldAccruedInterest_BlockLevel,'0.####')"/>
								</xsl:variable>

								<xsl:variable name="varAlloNetamount">
									<xsl:choose>
										<xsl:when test="contains(Side,'Buy')">
											<xsl:value-of select="(OldExecutedQuantity_BlockLevel * OldAvgPrice * AssetMultiplier) + (OldCommission_BlockLevel + OldSoftCommission_BlockLevel + OldOtherBrokerFees_BlockLevel + OldClearingBrokerFee_BlockLevel + OldStampDuty_BlockLevel + OldTransactionLevy_BlockLevel + OldClearingFee_BlockLevel + OldTaxOnCommissions_BlockLevel + OldMiscFees_BlockLevel + OldSecFee_BlockLevel + OldOccFee_BlockLevel + OldOrfFee_BlockLevel)"/>
										</xsl:when>
										<xsl:when test="contains(Side,'Sell')">
											<xsl:value-of select="(OldExecutedQuantity_BlockLevel * OldAvgPrice * AssetMultiplier) - (OldCommission_BlockLevel + OldSoftCommission_BlockLevel + OldOtherBrokerFees_BlockLevel + OldClearingBrokerFee_BlockLevel + OldStampDuty_BlockLevel + OldTransactionLevy_BlockLevel + OldClearingFee_BlockLevel + OldTaxOnCommissions_BlockLevel + OldMiscFees_BlockLevel + OldSecFee_BlockLevel + OldOccFee_BlockLevel + OldOrfFee_BlockLevel)"/>
										</xsl:when>
									</xsl:choose>
								</xsl:variable>

								<NetMoney>
									<xsl:choose>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="format-number(($varAlloNetamount + $varAccruedInterest),'0.####')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number($varAlloNetamount,'0.####')"/>
										</xsl:otherwise>
									</xsl:choose>
								</NetMoney>

								<xsl:variable name="varPositionEffect">
									<xsl:choose>
										<xsl:when test="(OldSide = 'Buy' or OldSide = 'Sell short' or OldSide='Buy to Open' or OldSide = 'Sell to Open')">
											<xsl:value-of select="'O'"/>
										</xsl:when>

										<xsl:when test="(OldSide = 'Sell' or Side = 'Buy to Close' or OldSide='Sell to Close')">
											<xsl:value-of select="'C'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:variable>

								<PositionEffect>
									<xsl:choose>
										<xsl:when test="(Asset='Equity' and IsSwapped='true') or Asset='EquityOption' ">
											<xsl:value-of select="$varPositionEffect"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</PositionEffect>

								<Text>
									<xsl:value-of select="''"/>
								</Text>

								<NumDaysInterest>
									<xsl:value-of select="''"/>
								</NumDaysInterest>

								<AccruedInterestAmt>
									<xsl:choose>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="$varAccruedInterest"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</AccruedInterestAmt>

								<xsl:variable name = "PRANA_FUND_NAME">
									<xsl:value-of select="AccountName"/>
								</xsl:variable>

								<xsl:variable name ="THIRDPARTY_FUND_CODE">
									<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
								</xsl:variable>
								<AllocAccount>
									<xsl:choose>
										<xsl:when test ="$THIRDPARTY_FUND_CODE != ''">
											<xsl:value-of select ="$THIRDPARTY_FUND_CODE"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="$PRANA_FUND_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</AllocAccount>

								<AllocAcctIDSource>
									<xsl:value-of select="''"/>
								</AllocAcctIDSource>

								<AlocQty>
									<xsl:value-of select="OldExecutedQuantity"/>
								</AlocQty>

								<IndividualAllocID>
									<xsl:value-of select="''"/>
								</IndividualAllocID>

								<xsl:variable name="THIRDPARTY_ProcessCode_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@ProcessCode"/>
								</xsl:variable>

								<xsl:variable name="varProcessCode">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_ProcessCode_NAME!=''">
											<xsl:value-of select="$THIRDPARTY_ProcessCode_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<ProcessCode>
									<xsl:choose>
										<xsl:when test="(Asset='Equity' and IsSwapped='true')">
											<xsl:value-of select="$varProcessCode"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>
									</xsl:choose>
								</ProcessCode>


								<xsl:variable name="THIRDPARTY_NestedPartyID_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@NestedPartyID"/>
								</xsl:variable>

								<xsl:variable name="varNestedPartyID">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_NestedPartyID_NAME!=''">
											<xsl:value-of select="$THIRDPARTY_NestedPartyID_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<NestedPartyID>
									<xsl:choose>
										<xsl:when test="(Asset='Equity' and IsSwapped='true') or Asset='EquityOption'">
											<xsl:value-of select="$varCounterParty"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varNestedPartyID"/>
										</xsl:otherwise>
									</xsl:choose>

								</NestedPartyID>

								<xsl:variable name="THIRDPARTY_NestedPartyIDSource_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@NestedPartyIDSource"/>
								</xsl:variable>

								<xsl:variable name="varNestedPartyIDSource">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_NestedPartyIDSource_NAME!=''">
											<xsl:value-of select="$THIRDPARTY_NestedPartyIDSource_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<NestedPartyIDSource>
									<xsl:choose>
										<xsl:when test="(Asset='Equity' and IsSwapped !='true') or Asset='FixedIncome' ">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varNestedPartyIDSource"/>
										</xsl:otherwise>
									</xsl:choose>

								</NestedPartyIDSource>

								<xsl:variable name="THIRDPARTY_NestedPartyRole_NAME">
									<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@NestedPartyRole"/>
								</xsl:variable>

								<xsl:variable name="varNestedPartyRole">
									<xsl:choose>
										<xsl:when test="$THIRDPARTY_NestedPartyRole_NAME!=''">
											<xsl:value-of select="$THIRDPARTY_NestedPartyRole_NAME"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<NestedPartyRole>
									<xsl:choose>
										<xsl:when test="(Asset='Equity' and IsSwapped !='true') or Asset='FixedIncome' ">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varNestedPartyRole"/>
										</xsl:otherwise>
									</xsl:choose>

								</NestedPartyRole>

								<AllocText>
									<xsl:value-of select="''"/>
								</AllocText>

								<xsl:variable name="varCommission">
									<xsl:value-of select="OldCommission + OldSoftCommission"/>
								</xsl:variable>
								<Commission>
									<xsl:value-of select="format-number(OldCommission,'0.####')"/>
								</Commission>

								<CommType>
									<xsl:value-of select="'3'"/>
								</CommType>

								<AllocCommissionAmount1>
									<xsl:value-of select="format-number(OldCommission,'0.####')"/>
								</AllocCommissionAmount1>

								<AllocCommissionAmountType1>
									<xsl:value-of select="'0'"/>
								</AllocCommissionAmountType1>

								<AllocCommissionAmountSubType1>
									<xsl:value-of select="'2'"/>
								</AllocCommissionAmountSubType1>

								<AllocCommissionBasis1>
									<xsl:value-of select="'3'"/>
								</AllocCommissionBasis1>



								<AllocAvgPx>
									<xsl:value-of select="OldAvgPrice"/>
								</AllocAvgPx>

								<xsl:variable name="varAllocNetamount">
									<xsl:choose>
										<xsl:when test="contains(OldSide,'Buy')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) + (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
										</xsl:when>
										<xsl:when test="contains(OldSide,'Sell')">
											<xsl:value-of select="(OldExecutedQuantity * OldAvgPrice * AssetMultiplier) - (OldCommission + OldSoftCommission + OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee)"/>
										</xsl:when>
									</xsl:choose>
								</xsl:variable>


								<AllocNetMoney>
									<xsl:choose>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="format-number(($varAllocNetamount + OldAccruedInterest),'0.####')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number($varAllocNetamount,'0.####')"/>
										</xsl:otherwise>
									</xsl:choose>
								</AllocNetMoney>



								<xsl:variable name="varFXRate">
									<xsl:choose>
										<xsl:when test="OldSettlCurrency != CurrencySymbol">
											<xsl:value-of select="OldFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="1"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name ="varNetAmount">
									<xsl:choose>
										<xsl:when test="$varFXRate=0">
											<xsl:value-of select="$varAllocNetamount"/>
										</xsl:when>

										<xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='M'">
											<xsl:value-of select="$varAllocNetamount * $varFXRate"/>
										</xsl:when>

										<xsl:when test="$varFXRate!=0 and OldFXConversionMethodOperator='D'">
											<xsl:value-of select="$varAllocNetamount div $varFXRate"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<AllocSettlCurrAmt>
									<xsl:choose>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="format-number(($varNetAmount + OldAccruedInterest),'0.####')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number($varNetAmount,'0.####')"/>
										</xsl:otherwise>
									</xsl:choose>
								</AllocSettlCurrAmt>

								<AllocSettlCurrency>
									<xsl:value-of select="OldSettlCurrency"/>
								</AllocSettlCurrency>

								<SettlCurrFxRate>
									<xsl:choose>
										<xsl:when test="OldSettlCurrency = CurrencySymbol">
											<xsl:value-of select="1"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="OldFXRate"/>
										</xsl:otherwise>
									</xsl:choose>
								</SettlCurrFxRate>

								<SettlCurrFxRateCalc>
									<xsl:value-of select="OldFXConversionMethodOperator"/>
								</SettlCurrFxRateCalc>

								<AllocAccruedInterestAmt>
									<xsl:choose>
										<xsl:when test="Asset='FixedIncome'">
											<xsl:value-of select="format-number(OldAccruedInterest,'0.####')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</AllocAccruedInterestAmt>

								<MiscFeeAmt>
									<xsl:value-of select="format-number((OldOtherBrokerFees + OldClearingBrokerFee + OldStampDuty + OldTransactionLevy + OldClearingFee + OldTaxOnCommissions + OldMiscFees + OldSecFee + OldOccFee + OldOrfFee),'0.####')"/>
								</MiscFeeAmt>

								<MiscFeeCurr>
									<xsl:value-of select="CurrencySymbol"/>
								</MiscFeeCurr>

								<MiscFeeType>
									<xsl:value-of select="'7'"/>
								</MiscFeeType>

								<MiscFeeBasis>
									<xsl:value-of select="0"/>
								</MiscFeeBasis>


								<EntityID>
									<xsl:value-of select="EntityID"/>
								</EntityID>

							</ThirdPartyFlatFileDetail>
						</xsl:if>
						<ThirdPartyFlatFileDetail>

							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<TaxLotState>
								<xsl:value-of select="'Allocated'"/>
							</TaxLotState>


							<AllocID>								
						      <xsl:value-of select ="concat(PBUniqueID,'N')"/>	
							</AllocID>

							<AllocTransType>								
							   <xsl:value-of  select="'0'"/>
							</AllocTransType>

							<AllocType>
								<xsl:value-of select="''"/>
							</AllocType>

							<SecondaryAllocID>
								<xsl:value-of select="''"/>
							</SecondaryAllocID>

							<RefAllocID>							
							  <xsl:value-of  select="''"/>
							</RefAllocID>

							<AllocCancReplaceReason>
								<xsl:value-of select="''"/>
							</AllocCancReplaceReason>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'Test_Constellation'"/>
							</xsl:variable>

							<xsl:variable name="PRANA_BROKER_NAME" select="CounterParty"/>

							<xsl:variable name="THIRDPARTY_BROKER_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@ThirdPartyBroker=$PRANA_BROKER_NAME]/@PranaBroker"/>
							</xsl:variable>

							<xsl:variable name="varCounterParty">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_BROKER_NAME != ''">
										<xsl:value-of select="$THIRDPARTY_BROKER_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_BROKER_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<BrokerID>
								<xsl:value-of select="$varCounterParty"/>
							</BrokerID>

							<AllocNoOrdersType >
								<xsl:value-of select="''"/>
							</AllocNoOrdersType >


							<ClOrdID>
								<xsl:value-of select="''"/>
							</ClOrdID>

							<Side>
								<xsl:choose>
									<xsl:when test ="Side = 'Buy' or Side = 'Buy to Open' or Side = 'Buy to Close'">
										<xsl:value-of  select="'1'"/>
									</xsl:when>
									<xsl:when test ="Side = 'Sell' or Side = 'Sell to Close'">
										<xsl:value-of  select="'2'"/>
									</xsl:when>
									<xsl:when test ="Side = 'Sell short' or Side = 'Sell to Open'">
										<xsl:value-of  select="'5'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>



							<Symbol>
								<xsl:choose>
									<xsl:when test="Asset = 'FixedIncome'">
										<xsl:value-of select="UnderlyingSymbol"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>


							<SymbolSfx>
								<xsl:value-of select="''"/>
							</SymbolSfx>

							<SecurityID>
								<xsl:choose>
									<xsl:when test="Asset='Equity' and CurrencySymbol ='USD' and CUSIP !=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:when test="Asset='Equity' and CurrencySymbol !='USD' and SEDOL !=''">
										<xsl:value-of select ="SEDOL"/>
									</xsl:when>
									<xsl:when test ="Asset = 'EquityOption' and OSIOptionSymbol !=''">
										<xsl:value-of select ="OSIOptionSymbol"/>
									</xsl:when>
									<xsl:when test ="Asset = 'FixedIncome' and CUSIP !=''">
										<xsl:value-of select="CUSIP"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Symbol"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityID>

							<SecurityIDSource>
								<xsl:choose>
									<xsl:when test="(Asset='Equity' or Asset='FixedIncome') and CurrencySymbol ='USD' and CUSIP !=''">
										<xsl:value-of select="'1'"/>
									</xsl:when>
									<xsl:when test="Asset='Equity' and CurrencySymbol !='USD' and SEDOL !=''">
										<xsl:value-of select ="'2'"/>
									</xsl:when>
									<xsl:when test ="Asset = 'EquityOption' and OSIOptionSymbol !=''">
										<xsl:value-of select ="'H'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityIDSource>


							<xsl:variable name="varCFICode">
								<xsl:choose>
									<xsl:when test ="Asset = 'EquityOption'">
										<xsl:choose>
											<xsl:when test="PutOrCall='PUT'">
												<xsl:value-of select="'OPXXCS'"/>
											</xsl:when>
											<xsl:when test="PutOrCall='CALL'">
												<xsl:value-of select="'OCXXCS'"/>
											</xsl:when>
										</xsl:choose>
									</xsl:when>
									<xsl:when test ="Asset = 'Future'">
										<xsl:value-of select="'FXXXXX'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<CFICode>
								<xsl:value-of select="$varCFICode"/>
							</CFICode>

							<SecurityType>
								<xsl:choose>
									<xsl:when test="Asset = 'Equity'">
										<xsl:value-of select="'CS'"/>
									</xsl:when>
									<xsl:when test="Asset = 'FixedIncome'">
										<xsl:value-of select="'CORP'"/>
									</xsl:when>
									<xsl:when test="Asset = 'Future'">
										<xsl:value-of select="'FUT'"/>
									</xsl:when>
									<xsl:when test ="Asset = 'EquityOption'">
										<xsl:value-of select ="'OPT'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecurityType>
							
				<xsl:variable name="varExpirationDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="ExpirationDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

							<xsl:variable name="EXP_Day" select="substring-before(substring-after($varExpirationDate,'/'),'/')"/>
							<xsl:variable name="EXP_Month" select="substring-before($varExpirationDate,'/')"/>
							<xsl:variable name="EXP_Year" select="substring-after(substring-after($varExpirationDate,'/'),'/')"/>

							<xsl:variable name="varMaturityDate">
								<xsl:choose>
									<xsl:when test ="Asset = 'EquityOption' or Asset='Future' or Asset ='FixedIncome'">
										<xsl:value-of select="concat($EXP_Year,$EXP_Month,$EXP_Day)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<MaturityDate>
								<xsl:value-of select="$varMaturityDate"/>
							</MaturityDate>
							
										
				<xsl:variable name="IssueDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="IssueDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

							<xsl:variable name="Issue_Day" select="substring-before(substring-after($IssueDate,'/'),'/')"/>
							<xsl:variable name="Issue_Month" select="substring-before($IssueDate,'/')"/>
							<xsl:variable name="Issue_Year" select="substring-after(substring-after($IssueDate,'/'),'/')"/>

							<xsl:variable name="varIssueDate">
								<xsl:choose>
									<xsl:when test ="Asset ='FixedIncome'">
										<xsl:value-of select="concat($Issue_Year,$Issue_Month,$Issue_Day)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<IssueDate>
								<xsl:value-of select="$varIssueDate"/>
							</IssueDate>

							<Factor>
								<xsl:choose>
									<xsl:when test ="Asset ='FixedIncome'">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</Factor>

							<StrikePrice>
								<xsl:choose>
									<xsl:when test ="Asset = 'EquityOption'">
										<xsl:value-of select="StrikePrice"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</StrikePrice>

							<StrikeCurrency>
								<xsl:value-of select="''"/>
							</StrikeCurrency>

							<StrikeMultiplier>
								<xsl:value-of select="''"/>
							</StrikeMultiplier>

							<StrikeValue>
								<xsl:value-of select="''"/>
							</StrikeValue>


							<xsl:variable name="PRANA_COUNTRY_NAME" select="CountryName"/>

							<xsl:variable name="THIRDPARTY_COUNTRY_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_CountryCodeMapping.xml')/CountryCodeMapping/PB[@Name=$PB_NAME]/CountryData[@PranaCountryCode=$PRANA_COUNTRY_NAME]/@PBCountryName"/>
							</xsl:variable>

							<xsl:variable name="varCountryOfIssue">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_COUNTRY_NAME != ''">
										<xsl:value-of select="$THIRDPARTY_COUNTRY_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PRANA_COUNTRY_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<CountryOfIssue>
								<xsl:value-of select="$varCountryOfIssue"/>
							</CountryOfIssue>


							<ContractMultiplier>
								<xsl:choose>

									<xsl:when test="Asset='EquityOption'">
										<xsl:value-of select="AssetMultiplier"/>
									</xsl:when>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="1"/>
									</xsl:when>
									<xsl:when test="AssetMultiplier !='1'">
										<xsl:value-of select="AssetMultiplier"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</ContractMultiplier>

							<xsl:variable name="varPutOrCall">
								<xsl:choose>
									<xsl:when test ="Asset = 'EquityOption'">
										<xsl:value-of select="PutOrCall"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<PutOrCall>
								<xsl:choose>
									<xsl:when test="$varPutOrCall='PUT'">
										<xsl:value-of select="'0'"/>
									</xsl:when>
									<xsl:when test="$varPutOrCall='CALL'">
										<xsl:value-of select="'1'"/>
									</xsl:when>
								</xsl:choose>
							</PutOrCall>

							<xsl:variable name="PRANA_MIC_NAME" select="Exchange"/>
							<xsl:variable name="varCCY" select="CurrencySymbol"/>

							<xsl:variable name="THIRDPARTY_MIC_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdPartyMIC_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@Exchange=$PRANA_MIC_NAME]/@PranaMICCode"/>
							</xsl:variable>

							<xsl:variable name="varSecurityExchange">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_MIC_NAME!=''">
										<xsl:value-of select="$THIRDPARTY_MIC_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<SecurityExchange>
								<xsl:value-of select="$varSecurityExchange"/>
							</SecurityExchange>

							<CouponRate>
								<xsl:choose>
									<xsl:when test ="Asset ='FixedIncome'">
										<xsl:value-of select="Coupon"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</CouponRate>

							<Issuer>
								<xsl:value-of select="''"/>
							</Issuer>

							<SecurityDesc>
								<xsl:value-of select="CompanyName"/>
							</SecurityDesc>

							<InterestAccrualDate>
								<xsl:value-of select="''"/>
							</InterestAccrualDate>

							<UnderlyingSymbol>
								<xsl:choose>
									<xsl:when test ="Asset = 'EquityOption'">
										<xsl:value-of select="UnderlyingSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</UnderlyingSymbol>

							<UnderlyingSecurityID>
								<xsl:value-of select="''"/>
							</UnderlyingSecurityID>

							<UnderlyingSecurityIDSource>
								<xsl:value-of select="''"/>
							</UnderlyingSecurityIDSource>

							<UnderlyingSecurityType>
								<xsl:value-of select="''"/>
							</UnderlyingSecurityType>

							<UnderlyingMaturityMonthYear>
								<xsl:value-of select="''"/>
							</UnderlyingMaturityMonthYear>

							<Quantity>
								<xsl:value-of select="CumQty"/>
							</Quantity>

							<QtyType>
								<xsl:choose>
									<xsl:when test="Asset='EquityOption' or Asset='FutureOption'">
										<xsl:value-of select="'1'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'0'"/>
									</xsl:otherwise>
								</xsl:choose>
							</QtyType>

							<PriceType>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="2"/>
									</xsl:otherwise>
								</xsl:choose>
							</PriceType>

							<AvgPx>
								<xsl:value-of select="AvgPrice"/>
							</AvgPx>

							<Currency>
								<xsl:value-of select="CurrencySymbol"/>
							</Currency>

							<AvgPxPrecision>
								<xsl:value-of select="''"/>
							</AvgPxPrecision>

							<xsl:variable name="THIRDPARTY_PartyID_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@PartyID"/>
							</xsl:variable>

							<xsl:variable name="varPartyID">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_PartyID_NAME!=''">
										<xsl:value-of select="$THIRDPARTY_PartyID_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PartyID>
								<xsl:value-of select="$varPartyID"/>
							</PartyID>

							<xsl:variable name="THIRDPARTY_PartyIDSource_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@PartyIDSource"/>
							</xsl:variable>

							<xsl:variable name="varPartyIDSource">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_PartyIDSource_NAME!=''">
										<xsl:value-of select="$THIRDPARTY_PartyIDSource_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<PartyIDSource>
								<xsl:value-of select="$varPartyIDSource"/>
							</PartyIDSource>

							<xsl:variable name="THIRDPARTY_PartyRole_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@PartyRole"/>
							</xsl:variable>

							<xsl:variable name="varPartyRole">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_PartyRole_NAME!=''">
										<xsl:value-of select="$THIRDPARTY_PartyRole_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<PartyRole>
								<xsl:value-of select="$varPartyRole"/>
							</PartyRole>

			 <xsl:variable name="varTradeDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="TradeDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>
              <TradeDate>
                <xsl:value-of select="concat(substring-after(substring-after($varTradeDate,'/'),'/'),substring-before($varTradeDate,'/'),substring-before(substring-after($varTradeDate,'/'),'/'))"/>
              </TradeDate>
							
				<xsl:variable name="varSettlementDate">
                <xsl:call-template name="DateFormat">
                  <xsl:with-param name="Date" select="SettlementDate">
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:variable>

							<SettlDate>
								<xsl:value-of select ="concat(substring-after(substring-after($varSettlementDate,'/'),'/'),substring-before($varSettlementDate,'/'),substring-before(substring-after($varSettlementDate,'/'),'/'))"/>
							</SettlDate>

							<BookingType>
								<xsl:choose>
									<xsl:when test="(Asset='Equity' and IsSwapped='true')">
										<xsl:value-of select="'1'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'0'"/>
									</xsl:otherwise>
								</xsl:choose>
							</BookingType>

							<xsl:variable name="varGrossTradeAmt">
								<xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier)"/>
							</xsl:variable>

							<GrossTradeAmt>
								<xsl:value-of select="format-number($varGrossTradeAmt,'0.####')"/>
							</GrossTradeAmt>

							<Concession>
								<xsl:value-of select="''"/>
							</Concession>

							<xsl:variable name="varAccruedInterest">
								<xsl:value-of select="format-number(AccruedInterest_BlockLevel,'0.####')"/>
							</xsl:variable>

							<xsl:variable name="varAlloNetamount">
								<xsl:choose>
									<xsl:when test="contains(Side,'Buy')">
										<xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier) + Commission_BlockLevel + SoftCommission_BlockLevel + OtherBrokerFees_BlockLevel + ClearingBrokerFee_BlockLevel + StampDuty_BlockLevel + TransactionLevy_BlockLevel + ClearingFee_BlockLevel + TaxOnCommissions_BlockLevel + MiscFees_BlockLevel + SecFee_BlockLevel + OccFee_BlockLevel + OrfFee_BlockLevel"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Sell')">
										<xsl:value-of select="(CumQty * AvgPrice * AssetMultiplier) - (Commission_BlockLevel + SoftCommission_BlockLevel + OtherBrokerFees_BlockLevel + ClearingBrokerFee_BlockLevel + StampDuty_BlockLevel + TransactionLevy_BlockLevel + ClearingFee_BlockLevel + TaxOnCommissions_BlockLevel + MiscFees_BlockLevel + SecFee_BlockLevel + OccFee_BlockLevel + OrfFee_BlockLevel)"/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>

							<NetMoney>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="format-number(($varAlloNetamount + $varAccruedInterest),'0.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number($varAlloNetamount,'0.####')"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetMoney>

							<xsl:variable name="varPositionEffect">
								<xsl:choose>
									<xsl:when test="(Side = 'Buy' or Side = 'Sell short' or Side='Buy to Open' or Side = 'Sell to Open')">
										<xsl:value-of select="'O'"/>
									</xsl:when>

									<xsl:when test="(Side = 'Sell' or Side = 'Buy to Close' or Side='Sell to Close')">
										<xsl:value-of select="'C'"/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>

							<PositionEffect>
								<xsl:choose>
									<xsl:when test="(Asset='Equity' and IsSwapped='true') or Asset='EquityOption' ">
										<xsl:value-of select="$varPositionEffect"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</PositionEffect>

							<Text>
								<xsl:value-of select="''"/>
							</Text>

							<NumDaysInterest>
								<xsl:value-of select="''"/>
							</NumDaysInterest>

							<AccruedInterestAmt>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="$varAccruedInterest"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</AccruedInterestAmt>

							<xsl:variable name = "PRANA_FUND_NAME">
								<xsl:value-of select="AccountName"/>
							</xsl:variable>

							<xsl:variable name ="THIRDPARTY_FUND_CODE">
								<xsl:value-of select ="document('../ReconMappingXml/ThirdParty_AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
							</xsl:variable>
							<AllocAccount>
								<xsl:choose>
									<xsl:when test ="$THIRDPARTY_FUND_CODE != ''">
										<xsl:value-of select ="$THIRDPARTY_FUND_CODE"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="$PRANA_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</AllocAccount>

							<AllocAcctIDSource>
								<xsl:value-of select="''"/>
							</AllocAcctIDSource>

							<AlocQty>
								<xsl:value-of select="OrderQty"/>
							</AlocQty>

							<IndividualAllocID>
								<xsl:value-of select="''"/>
							</IndividualAllocID>

							<xsl:variable name="THIRDPARTY_ProcessCode_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@ProcessCode"/>
							</xsl:variable>

							<xsl:variable name="varProcessCode">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_ProcessCode_NAME!=''">
										<xsl:value-of select="$THIRDPARTY_ProcessCode_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<ProcessCode>
								<xsl:choose>
									<xsl:when test="(Asset='Equity' and IsSwapped='true')">
										<xsl:value-of select="$varProcessCode"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'0'"/>
									</xsl:otherwise>
								</xsl:choose>
							</ProcessCode>


							<xsl:variable name="THIRDPARTY_NestedPartyID_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@NestedPartyID"/>
							</xsl:variable>

							<xsl:variable name="varNestedPartyID">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_NestedPartyID_NAME!=''">
										<xsl:value-of select="$THIRDPARTY_NestedPartyID_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<NestedPartyID>
								<xsl:choose>
									<xsl:when test="(Asset='Equity' and IsSwapped='true') or Asset='EquityOption'">
										<xsl:value-of select="$varCounterParty"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varNestedPartyID"/>
									</xsl:otherwise>
								</xsl:choose>

							</NestedPartyID>

							<xsl:variable name="THIRDPARTY_NestedPartyIDSource_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@NestedPartyIDSource"/>
							</xsl:variable>

							<xsl:variable name="varNestedPartyIDSource">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_NestedPartyIDSource_NAME!=''">
										<xsl:value-of select="$THIRDPARTY_NestedPartyIDSource_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<NestedPartyIDSource>
								<xsl:choose>
									<xsl:when test="(Asset='Equity' and IsSwapped !='true') or Asset='FixedIncome' ">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varNestedPartyIDSource"/>
									</xsl:otherwise>
								</xsl:choose>

							</NestedPartyIDSource>

							<xsl:variable name="THIRDPARTY_NestedPartyRole_NAME">
								<xsl:value-of select="document('../ReconMappingXml/ThirdParty_PartyID_Mapping.xml')/MICMapping/PB[@Name=$PB_NAME]/MICData[@PranaBroker=$PRANA_BROKER_NAME and @CCY=$varCCY]/@NestedPartyRole"/>
							</xsl:variable>

							<xsl:variable name="varNestedPartyRole">
								<xsl:choose>
									<xsl:when test="$THIRDPARTY_NestedPartyRole_NAME!=''">
										<xsl:value-of select="$THIRDPARTY_NestedPartyRole_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<NestedPartyRole>
								<xsl:choose>
									<xsl:when test="(Asset='Equity' and IsSwapped !='true') or Asset='FixedIncome' ">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varNestedPartyRole"/>
									</xsl:otherwise>
								</xsl:choose>

							</NestedPartyRole>

							<AllocText>
								<xsl:value-of select="''"/>
							</AllocText>

							<xsl:variable name="varCommission">
								<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
							</xsl:variable>
							<Commission>
								<xsl:value-of select="format-number(CommissionCharged,'0.####')"/>
							</Commission>

							<CommType>
								<xsl:value-of select="'3'"/>
							</CommType>

							<AllocCommissionAmount1>
								<xsl:value-of select="format-number(CommissionCharged,'0.####')"/>
							</AllocCommissionAmount1>

							<AllocCommissionAmountType1>
								<xsl:value-of select="'0'"/>
							</AllocCommissionAmountType1>

							<AllocCommissionAmountSubType1>
								<xsl:value-of select="'2'"/>
							</AllocCommissionAmountSubType1>

							<AllocCommissionBasis1>
								<xsl:value-of select="'3'"/>
							</AllocCommissionBasis1>



							<AllocAvgPx>
								<xsl:value-of select="AvgPrice"/>
							</AllocAvgPx>

							<xsl:variable name="varAllocNetamount">
								<xsl:choose>
									<xsl:when test="contains(Side,'Buy')">
										<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) + CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee"/>
									</xsl:when>
									<xsl:when test="contains(Side,'Sell')">
										<xsl:value-of select="(OrderQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>


							<AllocNetMoney>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="format-number(($varAllocNetamount + AccruedInterest),'0.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number($varAllocNetamount,'0.####')"/>
									</xsl:otherwise>
								</xsl:choose>
							</AllocNetMoney>



							<xsl:variable name="varFXRate">
								<xsl:choose>
									<xsl:when test="SettlCurrency != CurrencySymbol">
										<xsl:value-of select="FXRate_Taxlot"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="1"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name ="varNetAmount">
								<xsl:choose>
									<xsl:when test="$varFXRate=0">
										<xsl:value-of select="$varAllocNetamount"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='M'">
										<xsl:value-of select="$varAllocNetamount * $varFXRate"/>
									</xsl:when>

									<xsl:when test="$varFXRate!=0 and FXConversionMethodOperator='D'">
										<xsl:value-of select="$varAllocNetamount div $varFXRate"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<AllocSettlCurrAmt>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="format-number(($varNetAmount + AccruedInterest),'0.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="format-number($varNetAmount,'0.####')"/>
									</xsl:otherwise>
								</xsl:choose>
							</AllocSettlCurrAmt>

							<AllocSettlCurrency>
								<xsl:value-of select="SettlCurrency"/>
							</AllocSettlCurrency>

							<SettlCurrFxRate>
								<xsl:choose>
									<xsl:when test="SettlCurrency = CurrencySymbol">
										<xsl:value-of select="1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="FXRate_Taxlot"/>
									</xsl:otherwise>
								</xsl:choose>
							</SettlCurrFxRate>

							<SettlCurrFxRateCalc>
								<xsl:value-of select="FXConversionMethodOperator"/>
							</SettlCurrFxRateCalc>

							<AllocAccruedInterestAmt>
								<xsl:choose>
									<xsl:when test="Asset='FixedIncome'">
										<xsl:value-of select="format-number(AccruedInterest,'0.####')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</AllocAccruedInterestAmt>

							<MiscFeeAmt>
								<xsl:value-of select="format-number((OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee),'0.####')"/>
							</MiscFeeAmt>

							<MiscFeeCurr>
								<xsl:value-of select="CurrencySymbol"/>
							</MiscFeeCurr>

							<MiscFeeType>
								<xsl:value-of select="'7'"/>
							</MiscFeeType>

							<MiscFeeBasis>
								<xsl:value-of select="0"/>
							</MiscFeeBasis>


							<EntityID>
								<xsl:value-of select="EntityID"/>
							</EntityID>

						</ThirdPartyFlatFileDetail>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>