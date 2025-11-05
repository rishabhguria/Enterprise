<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<xsl:template name="GetSuffix">
		<xsl:param name="Suffix"/>
		<xsl:choose>
			<xsl:when test="$Suffix = 'JPY'">
				<xsl:value-of select="'-TSE'"/>
			</xsl:when>

			<xsl:when test="$Suffix = 'CAD'">
				<xsl:value-of select="'-TC'"/>
			</xsl:when>

			<xsl:when test="$Suffix = 'GBP'">
				<xsl:value-of select="'-LON'"/>
			</xsl:when>

			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select ="//Comparision">
				
				<!--<xsl:if test ="number(COL9) and  not(contains(COL26,'Cash'))='True' and COL2!='newgs' and COL2!='newfr' and COL2!='ntefr' and COL2!='ntegs'">-->
				<xsl:if test ="number(COL9) and number(COL22) and COL2!='newgs' and COL2!='newfr' and COL2!='ntefr' and COL2!='ntegs'">

					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='BNP']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:variable name="varCostBasis">
							<xsl:choose>
								<xsl:when test="COL4 = 'nbeu' or COL4 = 'cseu' or COL4 = 'kskr' or COL4 = 'csro'">
									<xsl:value-of select="COL12"/>
								</xsl:when>
								<xsl:when test ="COL4 = 'mbus'">
									<xsl:value-of select="COL13*100"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL13"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PB_Symbol">
							<xsl:value-of select = "COL5"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varNetPosition">
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select="''"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name="varSymbol">
							<xsl:value-of select ="translate(COL5, $vLowercaseChars_CONST, $vUppercaseChars_CONST)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test ="$varSymbol!= ''">
									<xsl:value-of select ="$varSymbol"/>
								</xsl:when>

								
								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<AvgPx>
							<xsl:choose>
								<xsl:when test ="number($varCostBasis) &gt; 0">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:when test ="number($varCostBasis) &lt; 0">
									<xsl:value-of select="$varCostBasis*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPx>

						<CUSIP>
							<xsl:value-of select="COL24"/>
						</CUSIP>

						<!--QUANTITY-->

						<xsl:choose>
							<xsl:when test="number($varNetPosition)">
								<Quantity>
									<xsl:value-of select="$varNetPosition"/>
								</Quantity>
							</xsl:when>
							<xsl:otherwise>
								<Quantity>
									<xsl:value-of select="0"/>
								</Quantity>
							</xsl:otherwise>
						</xsl:choose>

						<Side>
							<xsl:choose>
								<xsl:when test="COL9 &gt; 0">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="COL9 &lt; 0">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="COL4 = 'nbeu' or COL4 = 'cseu' or COL4 = 'kskr' or COL4 = 'csro'">
									<xsl:value-of select="COL10"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL11"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>