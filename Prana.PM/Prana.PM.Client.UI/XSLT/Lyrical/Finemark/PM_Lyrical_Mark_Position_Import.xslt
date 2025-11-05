<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<!--<xsl:template match="node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="@*">
		<xsl:attribute name="{local-name()}" namespace="{namespace-uri()}">
			<xsl:value-of select="replace(., '^\s+|\s+$', '')"/>
		</xsl:attribute>
	</xsl:template>-->




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

	<xsl:template name="varMonth">
		<xsl:param name="MonthName"/>
		<xsl:choose>
			<xsl:when test="$MonthName='June'">
				<xsl:value-of select="6"/>
			</xsl:when>
			<xsl:when test="$MonthName='Feb'">
				<xsl:value-of select="2"/>
			</xsl:when>
			<xsl:when test="$MonthName='Mar'">
				<xsl:value-of select="3"/>
			</xsl:when>
			<xsl:when test="$MonthName='Apr'">
				<xsl:value-of select="4"/>
			</xsl:when>
			<xsl:when test="$MonthName='May'">
				<xsl:value-of select="5"/>
			</xsl:when>
			<xsl:when test="$MonthName='Jun'">
				<xsl:value-of select="6"/>
			</xsl:when>
			<xsl:when test="$MonthName='Jul'">
				<xsl:value-of select="7"/>
			</xsl:when>
			<xsl:when test="$MonthName='Aug'">
				<xsl:value-of select="8"/>
			</xsl:when>
			<xsl:when test="$MonthName='Sep'">
				<xsl:value-of select="9"/>
			</xsl:when>
			<xsl:when test="$MonthName='Oct'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$MonthName='Nov'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$MonthName='Dec'">
				<xsl:value-of select="12"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>


	<xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

	<!-- Strips trailing whitespace characters from 'string' -->
	<xsl:template name="string-rtrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:variable name="length" select="string-length($string)" />

		<xsl:if test="$length &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, $length, 1))">
					<xsl:call-template name="string-rtrim">
						<xsl:with-param name="string" select="substring($string, 1, $length - 1)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading whitespace characters from 'string' -->
	<xsl:template name="string-ltrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:if test="string-length($string) &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, 1, 1))">
					<xsl:call-template name="string-ltrim">
						<xsl:with-param name="string" select="substring($string, 2)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading and trailing whitespace characters from 'string' -->
	<xsl:template name="string-trim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />
		<xsl:call-template name="string-rtrim">
			<xsl:with-param name="string">
				<xsl:call-template name="string-ltrim">
					<xsl:with-param name="string" select="$string" />
					<xsl:with-param name="trim"   select="$trim" />
				</xsl:call-template>
			</xsl:with-param>
			<xsl:with-param name="trim"   select="$trim" />
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template match="/">
		

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL2"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="varFundName" select="substring-before(substring-after(substring-after(//PositionMaster[contains(COL1,'THE DECESARIS FAMILY FNDN - LYR')]/COL1, ':'),' '),' ')"/>

				<xsl:variable name="varDate" select="substring-after(substring-before(//PositionMaster[contains(COL1,'SHTMTL20 LYRICAL')]/COL1, '-'),':')"/>

				<xsl:if test="number($varPosition) and not(contains(COL1,'Cash')) and COL1!=' Equities' and not(contains(COL1,'CASH')) and not(contains(COL1,'FINEMARK MONEY MARKET'))">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Fine Mark'"/>
						</xsl:variable>


						<xsl:variable name = "PB_SYMBOL_NAME">
							<xsl:value-of select ="''"/>
						</xsl:variable>
						
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name="varSymbol">							
							<xsl:value-of select="substring-before(normalize-space(COL1),'$')"/>
						</xsl:variable>-->

						<xsl:variable name="varSymbol">
							<xsl:value-of select="substring-before(COL1,'$')"/>
						</xsl:variable>

						<xsl:variable name="varNSymbol">
							<xsl:call-template name="string-ltrim">
								<xsl:with-param name="string" select="$varSymbol"/>
							</xsl:call-template>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="normalize-space(substring($varNSymbol,3,(string-length($varSymbol) - 6)))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="$varFundName"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varPosition &gt; 0">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varPosition &lt; 0">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<NetPosition>						
							<xsl:choose>
								<xsl:when test="$varPosition &gt; 0">
									<xsl:value-of select="$varPosition"/>
								</xsl:when>
								<xsl:when test="$varPosition &lt; 0">
									<xsl:value-of select="$varPosition * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>


						<xsl:variable name="varCostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL3"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$varCostBasis div $varPosition"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>

								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						
						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>