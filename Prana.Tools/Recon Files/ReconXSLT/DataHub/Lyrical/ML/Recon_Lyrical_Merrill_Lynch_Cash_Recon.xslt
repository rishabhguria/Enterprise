<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

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
			

			<xsl:for-each select ="//Comparision">
				<xsl:variable name="varCashLocal">
					<xsl:choose>
						<xsl:when test="contains(substring(COL1,24,1),'-')">
							<xsl:value-of  select="concat(substring(substring-before(substring-after(normalize-space(COL1),' '),'-'),1,string-length(substring-before(substring-after(normalize-space(COL1),' '),'-'))-2),'.',substring(substring-before(substring-after(normalize-space(COL1),' '),'-'),string-length(substring-before(substring-after(normalize-space(COL1),' '),'-'))-1))"/>
						</xsl:when>
						<xsl:when test="contains(substring(COL1,24,1),'+')">
							<xsl:value-of  select="concat(substring(substring-before(substring-after(normalize-space(COL1),' '),'+'),1,string-length(substring-before(substring-after(normalize-space(COL1),' '),'+'))-2),'.',substring(substring-before(substring-after(normalize-space(COL1),' '),'+'),string-length(substring-before(substring-after(normalize-space(COL1),' '),'+'))-1))"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="VarCashLocal1">
					<xsl:choose>
						<xsl:when test="contains(substring(COL1,24,1),'-')">
							<xsl:value-of  select="substring(substring-before(substring-after(substring-after(normalize-space(COL1),' '),'-'),'+'),1)"/>
						</xsl:when>
						<xsl:when test="contains(substring(COL1,24,1),'+')">
							<xsl:value-of  select="substring(substring-before(substring-after(substring-after(normalize-space(COL1),' '),'+'),'+'),1)"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="VarCashLocal13">
					<xsl:choose>
						<xsl:when test="contains(substring(COL1,24,1),'-')">
							<xsl:value-of  select="$VarCashLocal1 - $varCashLocal "/>
						</xsl:when>
						<xsl:when test="contains(substring(COL1,24,1),'+')">
							<xsl:value-of  select="$VarCashLocal1 + $varCashLocal "/>
						</xsl:when>
						<xsl:when test="contains(substring(COL1,24,1),'-') and contains(substring(COL1,37,1),'-')">
							<xsl:value-of  select="$VarCashLocal1 + $varCashLocal "/>
						</xsl:when>
						<xsl:when test="contains(substring(COL1,24,1),'-') and contains(substring(COL1,37,1),'+')">
							<xsl:value-of  select="$VarCashLocal1 - $varCashLocal "/>
						</xsl:when>
						<xsl:when test="contains(substring(COL1,24,1),'+') and contains(substring(COL1,37,1),'-')">
							<xsl:value-of  select="$VarCashLocal1 - $varCashLocal "/>
						</xsl:when>

					</xsl:choose>
				</xsl:variable>

				<xsl:variable name="varCashLocal4">
					<xsl:choose>
						<xsl:when test="contains(substring(COL1,50,1),'+')">
							<xsl:value-of select="concat(substring(COL1,37,11),'.',substring(COL1,48,2))"/>
						</xsl:when>
						<xsl:when test="contains(substring(COL1,50,1),'-')">
							<xsl:value-of select="concat(substring(COL1,37,11),'.',substring(COL1,48,2))"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>


				<xsl:variable name="VarCashLocal12">
					<xsl:choose>
						<xsl:when test="contains(substring(COL1,50,1),'-')">
							<xsl:value-of  select="$VarCashLocal13 - $varCashLocal4"/>
						</xsl:when>
						<xsl:when test="contains(substring(COL1,50,1),'+')">
							<xsl:value-of  select="$VarCashLocal13 + $varCashLocal4"/>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>



				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="$VarCashLocal12"/>
					</xsl:call-template>
				</xsl:variable>


				
				<xsl:choose>
					<xsl:when test="number($Cash)">
						<PositionMaster>

							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'Merrill Lynch'"/>
							</xsl:variable>

							<xsl:variable name="PB_FUND_NAME" select="substring-before(COL1,' ')"/>
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>
							<PortfolioAccount>
								<xsl:choose>
									<xsl:when test ="$PRANA_FUND_NAME!=''">
										<xsl:value-of select ="$PRANA_FUND_NAME"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select ="$PB_FUND_NAME"/>
									</xsl:otherwise>

								</xsl:choose>
							</PortfolioAccount>
							<xsl:variable name="varCurrency">
								<xsl:value-of select="'USD'"/>
							</xsl:variable>
							<Currency>
								<xsl:value-of select ="$varCurrency"/>
							</Currency>

							
							<OpeningBalanceDR>
								<xsl:choose>
									<xsl:when test="number($Cash)">
										<xsl:value-of select="$Cash"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceDR>

							<xsl:variable name="OpeningBalanceCR">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="''"/>
								</xsl:call-template>
							</xsl:variable>
							<OpeningBalanceCR>
								<xsl:choose>
									<xsl:when test="number($OpeningBalanceCR)">
										<xsl:value-of select="$OpeningBalanceCR"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</OpeningBalanceCR>


							<TradeDate>
								<xsl:value-of select ="''"/>
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

							<OpeningBalanceCR>
								<xsl:value-of select="0"/>
							</OpeningBalanceCR>

							<TradeDate>
								<xsl:value-of select ="''"/>
							</TradeDate>


						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>