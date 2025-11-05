<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail[AccountName != 'MSTR-Rebal Risk Analysis' and AccountName != 'SB####' and AccountName != 'AIRT-Risk Analysis' and AccountName != 'EAF-IB 5367' and AccountName != 'PANAFR-CIBC 7730' and AccountName != 'PANAFR-IB 3356' and AccountName != 'PANAFR-INTC EGP' and AccountName != 'PANAFR-INTC EUR' and AccountName != 'PANAFR-INTC GBP' and AccountName != 'PANAFR-INTC GHS' and AccountName != 'PANAFR-INTC KES' and AccountName != 'PANAFR-INTC MAD' and AccountName != 'PANAFR-INTC MUR' and AccountName != 'PANAFR-INTC NGN' and AccountName != 'PANAFR-INTC USD' and AccountName != 'PANAFR-INTC XOF' and AccountName != 'PANAFR-INTC ZAR']">
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
							<xsl:when test="AccountName ='AIRT-AGEN 0082' or AccountName ='AIRT-AGEN 0217' or AccountName ='AIRT-PIPR 7165' or AccountName ='AIRT-IB 8649'">
								<xsl:value-of select="'ATI-Air T'"/>
							</xsl:when>
							<xsl:when test="AccountName ='SPAG-IB 0034' or AccountName ='SPAG-IB 9812' or AccountName ='SPAG-PIPR 6076' or AccountName ='SPAG-PIPR 9500' ">
								<xsl:value-of select="'ATI-Space Age'"/>
							</xsl:when>
							<xsl:when test="AccountName ='PANAFR-TMIN 1520' or AccountName ='PANAFR-SBIC NGN' or AccountName ='PANAFR-SBIC GHS' or AccountName ='PANAFR-SBIC KES' or AccountName ='PANAFR-SBIC 6715' or AccountName ='PANAFR-SBSA-ZAR' or AccountName ='PANAFR-INTC NGN' or AccountName ='PANAFR-CIBC 7730'or AccountName ='PANAFR-INTC USD'or AccountName ='PANAFR-INTC EGP'or AccountName ='PANAFR-INTC 0907'or AccountName ='PANAFR-INTC ZAR'or AccountName ='PANAFR-INTC MAD'or AccountName ='PANAFR-INTC GHS'or AccountName ='PANAFR-INTC KES'or AccountName ='PANAFR-INTC GBP'or AccountName ='PANAFR-IB 3356'or AccountName ='PANAFR-INTC XOF'or AccountName ='PANAFR-INTC TZS'or AccountName ='PANAFR-INTC EUR'or AccountName ='PANAFR-INTC MUR'">
								<xsl:value-of select="'BCLAY-Pan-Africa'"/>
							</xsl:when>
							<xsl:when test="AccountName ='COI-CIBC 9246' or AccountName ='COI-GSCO 3141'">
								<xsl:value-of select="'BCLAY-CO I'"/>
								</xsl:when>
							<xsl:when test="AccountName ='GNTJMP ALER 5188' or AccountName ='GNTJMP-IB 1913'">
								<xsl:value-of select="'ATI-Giant Jump'"/>
								</xsl:when>
							<xsl:when test="AccountName ='BLUEBLT-MBT' or AccountName ='BLUEBOLT-IB 9032'">
								<xsl:value-of select="'ATI-Blue Bolt'"/>
							</xsl:when>
							<xsl:when test="AccountName ='COIII-CIBC 1374' or AccountName ='COIII-BTIG AV7R'">
								<xsl:value-of select="'BCLAY-CO III'"/>
							</xsl:when>
							<xsl:when test="AccountName ='SAYANI-STCH GHS' or AccountName ='SAYANI-STCH GBP' or AccountName ='SAYANI-STCH ZAR' or AccountName ='SAYANI-STCH MAD' or AccountName ='SAYANI-STCH USD' or AccountName ='SAYANI-STCH XOF' or AccountName ='SAYANI-STCH EGP' or AccountName ='SAYANI-STCH TZS'or AccountName ='SAYANI-STCH KES'">
								<xsl:value-of select="'BCLAY-SAYANI'"/>
							</xsl:when>
							<xsl:when test="AccountName ='LNGSHRT-BTIG AW5C' or AccountName ='LNGSHRT-IB 4437' ">
								<xsl:value-of select="'BCLAY-LNGSHRT'"/>
							</xsl:when>							
							<xsl:when test="AccountName ='SMID-CIBC 1399' or AccountName ='SMID-BTIG AV7V' ">
								<xsl:value-of select="'BCLAY-SMid'"/>
							</xsl:when>
							<xsl:when test="AccountName ='EAF-IB 5367' ">
								<xsl:value-of select="'BCCM-EAF'"/>
							</xsl:when>
							<xsl:when test="AccountName ='SCF-IB 0396' ">
								<xsl:value-of select="'BCCM-SCF'"/>
							</xsl:when>
							<xsl:when test="AccountName ='GSCVF-IB 7830' ">
								<xsl:value-of select="'BCCM GSCVF'"/>
							</xsl:when>
							<xsl:when test="AccountName ='GROVE-UBB' ">
								<xsl:value-of select="'AIRT GROVE'"/>
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
					<xsl:variable name="CostPrice">
						<xsl:choose>
							<xsl:when test="CurrencySymbol='ZAR'">
								<xsl:value-of select="AvgPrice * 100"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="AvgPrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<Position>
						<xsl:choose>
							<xsl:when test="Asset!='Cash'">
								<xsl:value-of select="Qty"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="$CostPrice"/>
							</xsl:otherwise>
						</xsl:choose>
					</Position>
					<Weight>
						<xsl:value-of select="''"/>
					</Weight>
					<MktPx>
						<xsl:value-of select="''"/>
					</MktPx>
					
					<CostPrice>
						<xsl:choose>
							<xsl:when test="Asset='Cash'">
								<xsl:value-of select="Qty"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="$CostPrice"/>
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