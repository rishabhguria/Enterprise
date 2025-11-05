<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail[contains(AccountName,'Catenary - JPM')]">
				<ThirdPartyFlatFileDetail>
					<!-- for system internal use -->
					<RowHeader>
						<xsl:value-of select="'true'"/>
					</RowHeader>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>				
					
					<xsl:variable name="PB_NAME" select="'Fidelity'"/>
					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>
					<xsl:variable name="THIRDPARTY_FUND_CODE">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					<Fundname>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Fundname>

					<Positionsdate>
						<xsl:value-of select="TradeDate"/>
					</Positionsdate>
					
					<Description>
						<xsl:value-of select="SecurityName"/>
					</Description>
					
					<Instrumenttype>						
						<xsl:value-of select="Asset"/>							
					</Instrumenttype>
					
					<BloombergTicker>
						<xsl:choose>
							<xsl:when test="Asset='Equity' and Currency='USD'">
								<xsl:value-of select="substring-before(BloombergSYmbol,' ')"/>
							</xsl:when>
							<xsl:when test="Asset='Equity' and Currency!='USD'">
								<xsl:value-of select="concat(substring-before(BloombergSYmbol,' '), ' ', substring-before(substring-after(BloombergSYmbol,' '),' '))"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption' and Currency='USD'">
								<xsl:value-of select="substring-before(BloombergSYmbol,' ')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="BloombergSYmbol"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</BloombergTicker>

					<BloombergYellowKey>

						<xsl:value-of select="BloombergSYmbol"/>

					</BloombergYellowKey>

					<Cusip>
						<xsl:value-of select="CUSIPSymbol"/>
					</Cusip>


					<Sedol>
						<xsl:value-of select="SEDOLSymbol"/>
					</Sedol>

					<Quantity>
						<!--<xsl:choose>
							<xsl:when test="Side='Long'">
								<xsl:value-of select="Qty"/>
							</xsl:when>
							<xsl:when test="Side='Short'">
								<xsl:value-of select="(Qty * -1)"/>
							</xsl:when>
						</xsl:choose>-->
						<xsl:value-of select="Qty"/>
					</Quantity>

					<Currency>
						<xsl:value-of select="Currency"/>
					</Currency>

					<Accountnumber>
						<xsl:value-of select="'102-53266'"/>
					</Accountnumber>

					

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>