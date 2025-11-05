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
				<xsl:if test="number(COL7)">
					<PositionMaster>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='CORMARK']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<xsl:variable name="PB_Symbol">
							<xsl:value-of select = "COL6"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='BTIG']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
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
							<xsl:value-of select ="COL2"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select="COL3"/>
						</PositionSettlementDate>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL8"/>
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

						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="string-length(COL5) = 21">
									<xsl:value-of select ="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL5"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test ="string-length(COL5) = 21">
									<xsl:value-of select="concat(COL5, 'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<xsl:variable name="varQuantity">
							<xsl:value-of select="number(COL7)"/>
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
						<xsl:variable name="varAssetType">
							<xsl:choose>
								<xsl:when test ="COL5 != '' and COL5 != '*' and string-length(COL5) = 21">
									<xsl:value-of select="'OPT'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'EQT'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varAssetType = 'EQT' and COL4 = 'BUY'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varAssetType = 'OPT' and COL4 = 'BUY'">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="$varAssetType = 'EQT' and COL4 = 'SEL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varAssetType = 'OPT' and COL4 = 'SEL'">
									<xsl:value-of select="'D'"/>
								</xsl:when>
								<xsl:when test="$varAssetType = 'EQT' and COL4 = 'SSL'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$varAssetType = 'OPT' and COL4 = 'SSL'">
									<xsl:value-of select="'C'"/>
								</xsl:when>
								<xsl:when test="($varAssetType = 'EQT' or $varAssetType = 'OPT') and COL4 = 'BTC'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select="COL6"/>
						</PBSymbol>

						<xsl:variable name="varCommision">
							<xsl:value-of select="COL9"/>
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

						<xsl:variable name="varFees">
							<xsl:value-of select="COL11"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test ='number($varFees) &lt; 0'>
									<xsl:value-of select ='$varFees*-1'/>
								</xsl:when>

								<xsl:when test ='number($varFees) &gt; 0'>
									<xsl:value-of select ='$varFees'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>

						<xsl:variable name="varStampDuty">
							<xsl:value-of select="COL10"/>
						</xsl:variable>
						
						<StampDuty>
							<xsl:choose>
								<xsl:when test ='number($varStampDuty) &lt; 0'>
									<xsl:value-of select ='$varStampDuty*-1'/>
								</xsl:when>

								<xsl:when test ='number($varStampDuty) &gt; 0'>
									<xsl:value-of select ='$varStampDuty'/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>
						
						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL16)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_BROKER_NAME">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='BOFA']/BrokerData[@PBBroker=$PB_BROKER_NAME]/@PranaBrokerID"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="$PRANA_BROKER_NAME!=''">
									<xsl:value-of select="$PRANA_BROKER_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>
