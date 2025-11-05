<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="PositionMaster">

				<xsl:variable name = "varInstrumentType" >
					<xsl:value-of select="translate(translate(COL21, ' ' , ''),'&quot;','')"/>
				</xsl:variable>
				<PositionMaster>
					<!--   Fund -->
					<!-- Column 1 mapped with Fund-->
					<!--<xsl:variable name = "varPortfolioID" >
							<xsl:value-of select="translate(COL4,'&quot;','')"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$varPortfolioID='21000000'">
								<FundName>
									<xsl:value-of select="'Letrp Value'"/>
								</FundName>
							</xsl:when>
							<xsl:when test="$varPortfolioID='000000005296005'">
								<FundName>
									<xsl:value-of select="'Letrol Value'"/>
								</FundName>								
							</xsl:when>
							
							<xsl:otherwise>
							<FundName>
									<xsl:value-of select="' '"/>
							</FundName>
							</xsl:otherwise >
						</xsl:choose >-->

					<!--   CUSIP -->
					<!-- Column 2 mapped with CUSIP-->
					<!--<CUSIP>
							<xsl:value-of select="translate(COL10,'&quot;','')"/>
						</CUSIP>
						<SEDOL>
							<xsl:value-of select="translate(COL9,'&quot;','')"/>
						</SEDOL>-->
					<!-- Prime Broker Symbol -->
					<PBAssetType>
						<xsl:value-of select="'Equities'"/>
					</PBAssetType>
					<PositionStartDate>
						<xsl:value-of select="translate(COL1,'&quot;','')"/>
					</PositionStartDate>
					<FundName>
						<xsl:value-of select="''"/>
					</FundName>
					<xsl:choose>
						<xsl:when  test="COL2 = 'BUY'">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose>					

					<xsl:choose>
						<xsl:when test ="COL3 &lt; 0 or COL3 &gt; 0 or COL3=0">
							<NetPosition>
								<xsl:value-of select="COL3"/>
							</NetPosition>
						</xsl:when>
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose>
					
					<Symbol>
						<xsl:value-of select="COL4"/>
					</Symbol>
					<xsl:choose>
						<xsl:when test ="COL5 &lt; 0 or COL5 &gt; 0 or COL5=0">
							<CostBasis>
								<xsl:value-of select="COL5"/>
							</CostBasis>
						</xsl:when>
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:variable name = "varPBCounterParty" >
						<xsl:value-of select="translate(translate(COL6,'&quot;',''), $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$varPBCounterParty = 'CIBC'">
							<CounterPartyID>
								<xsl:value-of select="'17'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'CITI'">
							<CounterPartyID>
								<xsl:value-of select="'1'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'CSFB'">
							<CounterPartyID>
								<xsl:value-of select="'5'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'FECC'">
							<CounterPartyID>
								<xsl:value-of select="'36'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'GSCO'">
							<CounterPartyID>
								<xsl:value-of select="'19'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'JEFF-PR'">
							<CounterPartyID>
								<xsl:value-of select="'27'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'LABS'">
							<CounterPartyID>
								<xsl:value-of select="'35'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'MLCO-PK'">
							<CounterPartyID>
								<xsl:value-of select="'33'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'MSCO'">
							<CounterPartyID>
								<xsl:value-of select="'24'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'PENS-JM'">
							<CounterPartyID>
								<xsl:value-of select="'42'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'PERS'">
							<CounterPartyID>
								<xsl:value-of select="'34'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'PERS-DR'">
							<CounterPartyID>
								<xsl:value-of select="'23'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'PERS-SC'">
							<CounterPartyID>
								<xsl:value-of select="'38'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'PETE'">
							<CounterPartyID>
								<xsl:value-of select="'13'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'RAJA'">
							<CounterPartyID>
								<xsl:value-of select="'9'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'RBCB'">
							<CounterPartyID>
								<xsl:value-of select="'14'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'SCMC'">
							<CounterPartyID>
								<xsl:value-of select="'12'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'SLKC'">
							<CounterPartyID>
								<xsl:value-of select="'20'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'SLKC-SL'">
							<CounterPartyID>
								<xsl:value-of select="'39'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'TRIS'">
							<CounterPartyID>
								<xsl:value-of select="'18'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'WACH'">
							<CounterPartyID>
								<xsl:value-of select="'41'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'WEED'">
							<CounterPartyID>
								<xsl:value-of select="'21'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'PERS-MX'">
							<CounterPartyID>
								<xsl:value-of select="'26'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'BARC'">
							<CounterPartyID>
								<xsl:value-of select="'44'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'NFSC'">
							<CounterPartyID>
								<xsl:value-of select="'31'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'ITGC'">
							<CounterPartyID>
								<xsl:value-of select="'32'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'CANT'">
							<CounterPartyID>
								<xsl:value-of select="'30'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'SLKC-JK'">
							<CounterPartyID>
								<xsl:value-of select="'22'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'SALI'">
							<CounterPartyID>
								<xsl:value-of select="'46'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'GRIS'">
							<CounterPartyID>
								<xsl:value-of select="'48'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'MLCO-BK'">
							<CounterPartyID>
								<xsl:value-of select="'47'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'MLCO'">
							<CounterPartyID>
								<xsl:value-of select="'40'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:when test="$varPBCounterParty = 'INCA'">
							<CounterPartyID>
								<xsl:value-of select="'30'"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:otherwise>
							<CounterPartyID>
								<xsl:value-of select="'-1'"/>
							</CounterPartyID>
						</xsl:otherwise >
					</xsl:choose >

					<xsl:choose>
						<xsl:when test ="COL7 &lt; 0 or COL7 &gt; 0 or COL7=0">
							<Commission>
								<xsl:value-of select="COL7"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>
					
					<PBSymbol>
						<xsl:value-of select="COL4"/>
					</PBSymbol>

				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>
