<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName != 'Rebal Risk Analysis' and AccountName != 'SB####']">
				<ThirdPartyFlatFileDetail>
					<!-- for system internal use -->
					<RowHeader>
						<xsl:value-of select="'true'"/>
					</RowHeader>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>				

					<Advisor>
						<xsl:choose>
							<xsl:when test="AccountName ='AIRT-AGEN 0082' or AccountName ='AIRT-AGEN 0217' or AccountName ='AIRT-IB 6160' or AccountName ='AIRT-PIPR 7165'">
								<xsl:value-of select="'ATI-Air T'"/>
							</xsl:when>
							<xsl:when test="AccountName ='SPAG-IB 0034' or AccountName ='SPAG-IB 9812' or AccountName ='SPAG-PIPR 6076' ">
								<xsl:value-of select="'ATI-Space Age'"/>
							</xsl:when>
							<xsl:when test="AccountName ='AOII-AGEN 0095' or AccountName ='AOII-CIBC 0472' ">
								<xsl:value-of select="'BCCM-AO II'"/>
							</xsl:when>							
							<xsl:when test="AccountName ='PANAFR-AGEN 0097' or AccountName ='PANAFR-CIBC 4860' or AccountName ='PANAFR-CIBC 1338' or AccountName ='PANAFR-SBIC 0001' or AccountName ='PANAFR-SBIC 0002' or AccountName ='PANAFR-SBIC 0003' or AccountName ='PANAFR-SBIC 6715' or AccountName ='PANAFR-SBSA-0001' or AccountName ='PANAFR-SBSA 9706'">
								<xsl:value-of select="'BCLAY-Pan-Africa'"/>
							</xsl:when>
							<xsl:when test="AccountName ='COI-CIBC 9246' or AccountName ='COI-GSCO 3141'">
								<xsl:value-of select="'BCLAY-CO I'"/>
							</xsl:when>
							<xsl:when test="AccountName ='COIII-CIBC 1374' or AccountName ='COIII-GSCO 4449'">
								<xsl:value-of select="'BCLAY-CO III'"/>
							</xsl:when>
							<xsl:when test="AccountName ='MSTR-GSCO 1751' ">
								<xsl:value-of select="'BCLAY-Master'"/>
							</xsl:when>							
							<xsl:when test="AccountName ='SMID-CIBC 1399' or AccountName ='SMID-GSCO 0924' ">
								<xsl:value-of select="'BCLAY-SMid'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Advisor>

					<xsl:variable name="PB_NAME" select="'Bloomberg'"/>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="translate(AccountName,'–','-')"/>
					</xsl:variable>
					<xsl:variable name="THIRDPARTY_FUND_CODE">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					<PortfolioName>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</PortfolioName>

					<Issuer>
						<xsl:value-of select ="Issuer"/>
					</Issuer>	
					<Security>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSISymbol"/>
							</xsl:when>
							<xsl:when test="BBCode!='*'">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:when test ="Asset = 'Equity' and CurrencySymbol = 'USD'">
								<xsl:value-of select ="concat(Symbol,' ','US',' ','EQUITY')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="BBCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</Security>
					<Sedol>
						<xsl:value-of select="SEDOL"/>
					</Sedol>
					<Cusip>
						<xsl:value-of select="CUSIP"/>
					</Cusip>
					<ISIN>
						<xsl:value-of select="ISIN"/>
					</ISIN>
					<SecurityName>
						<xsl:value-of select="FullSecurityName"/>
					</SecurityName>
					<Position>
						<xsl:value-of select="Qty"/>
					</Position>
					<Weight>
						<xsl:value-of select="''"/>
					</Weight>
					<MktPx>
						<xsl:value-of select="''"/>
					</MktPx>

					<CostPrice>
					<xsl:choose>
						<xsl:when test="CurrencySymbol='ZAR'">
							<xsl:value-of select="AvgPrice * 100"/>
						</xsl:when>
						
						<xsl:otherwise>
							<xsl:value-of select="AvgPrice"/>
						</xsl:otherwise>
					</xsl:choose>

					</CostPrice>
				
					<xsl:variable name="TradeDate" select="substring-before(TradeDate,'T')"/>

					<AsOfDate>
						<xsl:value-of select="concat(substring-before(substring-after($TradeDate,'-'),'-'),'/',substring-after(substring-after($TradeDate,'-'),'-'),'/',substring-before($TradeDate,'-'))"/>
					</AsOfDate>

					<NewClassification>
						<xsl:value-of select="Asset"/>
					</NewClassification>
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>
		
				</xsl:for-each>
			
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>