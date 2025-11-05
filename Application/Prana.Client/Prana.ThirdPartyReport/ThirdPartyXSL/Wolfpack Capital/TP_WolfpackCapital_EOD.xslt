<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>

			<ThirdPartyFlatFileDetail>
				<RowHeader>
					<xsl:value-of select="'false'"/>
				</RowHeader>
				<TaxLotState>
					<xsl:value-of select="TaxLotState"/>
				</TaxLotState>

				<type>
					<xsl:value-of select="'Type'"/>
				</type>
				<clienttradeid>
					<xsl:value-of select="'Client Trade ID'"/>
				</clienttradeid>
				<timestamp>
					<xsl:value-of select="'Timestamp'"/>
				</timestamp>
				<date>
					<xsl:value-of select="'Date'"/>
				</date>
				<accountid>
					<xsl:value-of select="'Account ID'"/>
				</accountid>
				<quantity>
					<xsl:value-of select="'Quantity'"/>
				</quantity>
				<price>
					<xsl:value-of select="'Price'"/>
				</price>
				<instrumentidentifier>
					<xsl:value-of select="'Instrument Identifier'"/>
				</instrumentidentifier>
				<instrumentidentifiertype>
					<xsl:value-of select="'Instrument Identifier Type'"/>
				</instrumentidentifiertype>
				<instrumentcountry>
					<xsl:value-of select="'Instrument Country'"/>
				</instrumentcountry>
				<instrumentcurrency>
					<xsl:value-of select="'Instrument Currency'"/>
				</instrumentcurrency>
				<sidedirection>
					<xsl:value-of select="'Side Direction'"/>
				</sidedirection>
				<capacity>
					<xsl:value-of select="'Capacity'"/>
				</capacity>
				<feescommission>
					<xsl:value-of select="'Fees Commission'"/>
				</feescommission>
				<execmpid>
					<xsl:value-of select="'Exec MPID'"/>
				</execmpid>
				<contrampid>
					<xsl:value-of select="'Contra MPID'"/>
				</contrampid>
				<contraclearingnum>
					<xsl:value-of select="'Contra Clearing Num'"/>
				</contraclearingnum>
				<sideposition>
					<xsl:value-of select="'Side Position'"/>
				</sideposition>
				<sidequalifier>
					<xsl:value-of select="'Side Qualifier'"/>
				</sidequalifier>
				<fixedincomeaccruedinterest>
					<xsl:value-of select="'Fixed Income Accrued Interest'"/>
				</fixedincomeaccruedinterest>
				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>
					<RowHeader>
						<xsl:value-of select="'false'"/>
					</RowHeader>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<type>
						<xsl:value-of select="'away_trade'"/>
					</type>
					<clienttradeid>
						<xsl:value-of select="PBUniqueID"/>
					</clienttradeid>
					<timestamp>
						<xsl:value-of select="''"/>
					</timestamp>
					<date>
						<xsl:value-of select="TradeDate"/>
					</date>
					
						<xsl:variable name="PB_NAME">
					     <xsl:value-of select="'Clearstreet'"/>
					   </xsl:variable>

					   <xsl:variable name = "PRANA_FUND_NAME">
					     <xsl:value-of select="AccountName"/>
					   </xsl:variable>
					   <xsl:variable name ="PB_FUND_NAME">
					     <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PRANA_FUND_NAME]/@PranaFund"/>
					   </xsl:variable>

					<accountid>
					 <xsl:value-of select="AccountNo"/>
					</accountid>
					
					<quantity>
						<xsl:value-of select="ExecutedQty"/>
					</quantity>
					<price>
						<xsl:value-of select="AveragePrice"/>
					</price>
					<instrumentidentifier>
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</instrumentidentifier>
					
					<instrumentidentifiertype>
					<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'ticker'"/>
							</xsl:when>
							
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'osi'"/>
							</xsl:when>
							<xsl:when test="CUSIP!=''">
								<xsl:value-of select="'cusip'"/>
							</xsl:when>
							<xsl:when test="SEDOL!=''">
								<xsl:value-of select="'sedol'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</instrumentidentifiertype>
					
						 <xsl:variable name = "varCountry">
					     <xsl:value-of select="Country"/>
					   </xsl:variable>
					
					   <xsl:variable name ="PB_Country">
					     <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CountryCodeMappings.xml')/CountryCodeMapping/PB[@Name=$PB_NAME]/CountryData[@PranaCountryCode=$varCountry]/@PBCountryName"/>
					   </xsl:variable>
					<instrumentcountry>
						 <xsl:choose>
					      <xsl:when test ="$PB_Country!= ''">
					        <xsl:value-of select ="$PB_Country"/>
					      </xsl:when>
					      <xsl:otherwise>
					        <xsl:value-of select ="$varCountry"/>
					      </xsl:otherwise>
					    </xsl:choose>
					</instrumentcountry>
					
						<xsl:variable name ="varCurrency">
					     <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CountryCodeMappings.xml')/CountryCodeMapping/PB[@Name=$PB_NAME]/CountryData[@PranaCountryCode=$varCountry]/@PBCurrency"/>
					   </xsl:variable>
					<instrumentcurrency>
						 <xsl:value-of select ="CurrencySymbol"/>
					</instrumentcurrency>
					
					<sidedirection>
					<xsl:choose>
    <!-- Equities -->
    <xsl:when test="Asset = 'Equity'">
        <xsl:choose>
            <xsl:when test="Side = 'Sell short' or Side = 'Sell'">
                <xsl:value-of select="'sell'" />
            </xsl:when>
            <xsl:when test="Side = 'Buy' or Side = 'Buy to Close'">
                <xsl:value-of select="'buy'" />
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="''" />
            </xsl:otherwise>
        </xsl:choose>
    </xsl:when>

    <!-- Equity Option -->
    <xsl:when test="Asset = 'EquityOption'">
        <xsl:choose>
            <xsl:when test="Side = 'Sell to Open' or Side = 'Sell to Close'">
                <xsl:value-of select="'sell'" />
            </xsl:when>
            <xsl:when test="Side = 'Buy to Open' or Side = 'Buy to Close'">
                <xsl:value-of select="'buy'" />
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="''" />
            </xsl:otherwise>
        </xsl:choose>
    </xsl:when>

    <!-- Default case if asset type is not handled -->
    <xsl:otherwise>
        <xsl:value-of select="''" />
    </xsl:otherwise>
</xsl:choose>

					</sidedirection>
					
					<capacity>
						<xsl:value-of select="'agency'"/>
					</capacity>
						
					<feescommission>
					  <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</feescommission>
					
					 <xsl:variable name="PRANA_COUNTERPARTY">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name="PB_COUNTERPARTY">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name = 'GS']/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@PBBroker"/>
          </xsl:variable>

          <xsl:variable name="varCounterParty">
            <xsl:choose>
              <xsl:when test="$PB_COUNTERPARTY = ''">
                <xsl:value-of select="$PRANA_COUNTERPARTY"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PB_COUNTERPARTY"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          
					<execmpid>
				            <xsl:value-of select="$varCounterParty"/>
					</execmpid>
					
						
					<contrampid>
						 <xsl:value-of select="$varCounterParty"/>
					</contrampid>
					
					
					 <xsl:variable name = "varCurrencySymbol">
					     <xsl:value-of select="CurrencySymbol"/>
					   </xsl:variable>
						<xsl:variable name ="varCountryNumber">
					     <xsl:value-of select ="document('../ReconMappingXml/ThirdParty_CountryCodeMappings.xml')/CountryCodeMapping/PB[@Name=$PB_NAME]/CountryData[@PBCurrency=$varCurrencySymbol]/@PBCountryNumber"/>
					   </xsl:variable>
					<contraclearingnum>
							<xsl:choose>
					      <xsl:when test ="number($varCountryNumber)!= ''">
					        <xsl:value-of select ="$varCountryNumber"/>
					      </xsl:when>
					      <xsl:otherwise>
					        <xsl:value-of select ="$varCountryNumber"/>
					      </xsl:otherwise>
					    </xsl:choose>
					</contraclearingnum>
					
					<sideposition>
						<xsl:choose>
						<!-- Open side actions -->
						<xsl:when test="(Side='Buy to Open' or Side='Sell to Open' or Side='Buy' or Side='Sell short') and Asset='EquityOption'">
							<xsl:value-of select="'open'" />
						</xsl:when>
					
						<!-- Close side actions -->
						<xsl:when test="(Side='Sell to Close' or Side='Buy to Close' or Side='Sell')  and Asset='EquityOption'">
							<xsl:value-of select="'close'" />
						</xsl:when>
					
						<!-- Default case -->
						<xsl:otherwise>
							<xsl:value-of select="''" />
						</xsl:otherwise>
					</xsl:choose>
					</sideposition>
					
					<sidequalifier>
						<xsl:choose>							
							<xsl:when test="Side='Sell short' and Asset='Equity' ">
								<xsl:value-of select="'short'"/>
							</xsl:when>						
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</sidequalifier>
					
					<fixedincomeaccruedinterest>
						<xsl:value-of select="''"/>
					</fixedincomeaccruedinterest>
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>
