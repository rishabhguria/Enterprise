<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail[(AccountName='MAC Fund I LP A Partnership' or AccountName='Sloman' 
			or AccountName='RT6-400014 - Beck Sloman' or AccountName='RT6-400015 - Matti Sloman' or AccountName='RT6-400016 - Jim Sloman Trust' 
			or AccountName='RT6-400017 - Nora Devlin Trust' or AccountName='RT6-930002 - IRA-Jim Sloman' 
			or AccountName='RT6-930003 - IRA-Nora Devlin'
			
			or AccountName='RT6 400022 ALEXANDER WHELAN R' or AccountName='RT6 400023 CHARLES R PALMER' or AccountName='RT6 400024 JOHN N IRWIN'
			or AccountName='RT6 400025 JOHN M CEFALY' or AccountName='RT6 400026 OLIVER M RUTHERFURD' or AccountName='RT6 400027 BUTTERFIELD LORRAINE L GRACE'
			or AccountName='RT6 400028 BUTTERFIELD ANN GRACE KELLY' or AccountName='RT6 400029 BUTTERFIELD TR BERMUDA' 
			or AccountName='RT6 400030 BRIAN M SMITH CAROL'
			or AccountName='Alec Rutherfurd' or AccountName='RT6-400043 - Atlas Health Fund LP'
			
			)]">
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
							<xsl:when test="contains(Symbol,'O:')">
								<xsl:value-of select="OSISymbol"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>

					</Symbol>

					<Exchange>
						<xsl:choose>
							<xsl:when test="contains(Symbol,'O:')">
								<xsl:value-of select="'OPRA'"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Exchange>
					<AccountType>
						<xsl:value-of select="''"/>
					</AccountType>					
					<NoOfShares>						
						<xsl:value-of select="format-number(Qty,'#.##')"/>
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