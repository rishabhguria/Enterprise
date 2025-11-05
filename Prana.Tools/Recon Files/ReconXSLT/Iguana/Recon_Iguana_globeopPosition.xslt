<?xml version="1.0" encoding="utf-8"?>

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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//Comparision">
				<xsl:if test="number(COL7)">
					<PositionMaster>				


						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='LCM']/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='LCM']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>


						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<AccountName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME=''">
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						

						<xsl:variable name="varMarkPrice">
							<xsl:choose>
								<xsl:when test="number(COL7)!=0">
									<xsl:value-of select="COL11 div COL7"/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when  test="number(normalize-space(COL7)) ">
									<xsl:value-of select="COL7"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>
						
						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number($varMarkPrice) &gt; 0">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:when test="number($varMarkPrice) &lt; 0">
									<xsl:value-of select="$varMarkPrice*-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<MarketValue>
							<xsl:choose>
								<xsl:when test="number(COL11)">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="number(COL12) ">
									<xsl:value-of select="COL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>

						<Description>
							<xsl:value-of select="COL5"/>
						</Description>

						<Side>
							<xsl:choose>
								<xsl:when test="number(COL7) &gt; 0 ">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="number(COL7) &lt; 0 ">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
							</xsl:choose>
						</Side>
						
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
