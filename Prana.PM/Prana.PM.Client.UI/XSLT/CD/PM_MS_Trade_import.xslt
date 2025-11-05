<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime firstFriday= new DateTime(year, month, 1);
		while (firstFriday.DayOfWeek != DayOfWeek.Friday)
		{
		firstFriday = firstFriday.AddDays(1);
		}
		return firstFriday.ToString();
		}
	</msxsl:script>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL34) and COL26='Trade'">
					<PositionMaster>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPB_Name]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<PositionStartDate>
							<xsl:value-of select ="COL8"/>
						</PositionStartDate>

						<!--<PositionSettlementDate>
							<xsl:value-of select="COL36"/>
						</PositionSettlementDate>-->

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL49"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="number($varAvgPrice)" >
									<xsl:value-of select ="$varAvgPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="normalize-space(COL19)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>




						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>



						<xsl:variable name="varQuantity">
							<xsl:value-of select="COL34"/>
						</xsl:variable>

						<NetPosition>
							<xsl:choose>
								<xsl:when test ='number($varQuantity) &lt; 0'>
									<xsl:value-of select ='$varQuantity*-1'/>
								</xsl:when>
								<xsl:when test ='number($varQuantity) &gt; 0'>
									<xsl:value-of select ='$varQuantity'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ='0'/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL30)"/>
						</xsl:variable>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test ="$varSide='S'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="$varSide='B'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select="COL17"/>
						</PBSymbol>


						<xsl:variable name="varCommision">
							<xsl:value-of select="COL51"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($varCommision) &lt; 0'>
									<xsl:value-of select ='$varCommision*-1'/>
								</xsl:when>

								<xsl:when test ='number($varCommision) &gt; 0'>
									<xsl:value-of select ='$varCommision'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="varStamp">
							<xsl:value-of select="COL52"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test ='number($varStamp) &lt; 0'>
									<xsl:value-of select ='$varStamp*-1'/>
								</xsl:when>

								<xsl:when test ='number($varStamp) &gt; 0'>
									<xsl:value-of select ='$varStamp'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>

