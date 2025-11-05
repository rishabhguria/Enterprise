<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
	<xsl:output method="xml" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select="//Comparision">
				
				<xsl:variable name="CashBase">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL9)"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:choose>
					
					<xsl:when test="number($CashBase)">
						<PositionMaster>
							
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>
							
							<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
							
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>
							
							<PortfolioAccount>
								<xsl:choose>
									<xsl:when test="$PRANA_FUND_NAME!=''">
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</PortfolioAccount>

							<Symbol>
								<xsl:value-of select="normalize-space(COL13)"/>
							</Symbol>
							
							<Currency>
								<xsl:value-of select="COL13"/>
							</Currency>
							
							<OpeningBalanceDR>
								<xsl:choose>
									<xsl:when test="number($CashBase)">
										<xsl:value-of select="$CashBase"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceDR>
							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>
						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>
							<PortfolioAccount>
								<xsl:value-of select="''"/>
							</PortfolioAccount>
							<Currency>
								<xsl:value-of select="''"/>
							</Currency>
							<OpeningBalanceDR>
								<xsl:value-of select="0"/>
							</OpeningBalanceDR>
							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>
						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>