<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='MAC Fund I LP A Partnership' or AccountName='RT6-930003 - IRA-Nora Devlin' or AccountName='RT6-930002 - IRA-Jim Sloman' or AccountName='RT6-400017 - Nora Devlin Trust' or AccountName='RT6-400016 - Jim Sloman Trust' or AccountName='RT6-400015 - Matti Sloman' or AccountName='RT6-400014 - Beck Sloman' or AccountName='RT6 930001 - Donald Besser SEP IRA' or AccountName='RT6 400012 - Donald E Besser' or AccountName='RT6 400011 - Manchester Alpha Fund LP' or AccountName='RT6 400009 - JEB Partners' or AccountName='RT6 400008 - Manchester Explorer LP' or AccountName='RT6 400007 - Morgan C Frank' or AccountName='RT6 400006 - James E Besser']">
				<ThirdPartyFlatFileDetail>
					<!-- for system internal use -->
					<RowHeader>
						<xsl:value-of select="'true'"/>
					</RowHeader>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>
					<xsl:variable name="PB_NAME" select="'EOD_Fidelity'"/>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>
					<xsl:variable name="THIRDPARTY_FUND_CODE">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					<AccountNo>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountNo>
					<Symbol>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSISymbol"/>
							</xsl:when>
							<!--<xsl:when test="BBCode!='*'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test ="Asset = 'Equity' and CurrencySymbol = 'USD'">
								<xsl:value-of select ="Symbol"/>
							</xsl:when>-->
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>

					</Symbol>

					<Exchange>
						<xsl:value-of select="''"/>
					</Exchange>
					<AccountType>
						<xsl:value-of select="''"/>
					</AccountType>
					
					<PositionSide>
						<xsl:value-of select="PositionSide"/>
					</PositionSide>
					
					<NoOfShares>
						<xsl:value-of select="Qty"/>
					</NoOfShares>
					<ClosingPrice>
						<xsl:value-of select="format-number(AvgPrice,'#.0000')"/>
					</ClosingPrice>
					<EntryPrice>
						<xsl:value-of select="format-number(AvgPrice,'#.0000')"/>
					</EntryPrice>
					
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>