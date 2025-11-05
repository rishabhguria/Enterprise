<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName!='Rebal Risk Analysis']">
				<ThirdPartyFlatFileDetail>
					<!-- for system internal use -->
					<RowHeader>
						<xsl:value-of select="'true'"/>
					</RowHeader>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>
					<xsl:variable name="PB_NAME" select="'Bloomberg'"/>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="translate(AccountName,'–','-')"/>
					</xsl:variable>
					<xsl:variable name="THIRDPARTY_FUND_CODE">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
										
					<Advisor>
						<xsl:choose>
							<xsl:when test="AccountName ='F1446160 – AirT' or AccountName ='AirT–AGEN MHX000217' or AccountName ='AirT–AGEN AXT000082' or AccountName ='AirT–PIPR T6A017165' or AccountName ='SAIC–IB U1459812' or AccountName ='SAIC–IB U1460034' or AccountName ='SAIC–PIPR T6A016076' ">
								<xsl:value-of select="'Air T Inc'"/>
							</xsl:when>
							<xsl:when test="AccountName ='AO II–AGEN AXT000095'or AccountName ='Pan Africa–AGEN AXT000097'">
								<xsl:value-of select="'BCCM Advisors'"/>
							</xsl:when>
							<xsl:when test="AccountName ='SMid–GSCO 002680924' or AccountName ='CO III–GSCO 00258449' or AccountName ='CO I–GSCO 002513141' or AccountName ='Master–GSCO 002491751'">
								<xsl:value-of select="'Blue Clay Capital Management LLC'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select="'AXT000082 – AirT'"/>-->
					</Advisor>

					<PortfolioName>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
						<!--<xsl:value-of select="'AXT000082 – AirT'"/>-->
					</PortfolioName>
					
					<Security>
						<!--<xsl:choose>
							<xsl:when test="contains(BBCode,'Curncy')">
								<xsl:value-of select="BBCode"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(substring-before(BBCode,' '),' ',substring-before(substring-after(BBCode,' '),' '))"/>
							</xsl:otherwise>
						</xsl:choose>-->
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
						<xsl:value-of select="AvgPrice"/>
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