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
					<xsl:value-of select="translate(COL7,'&quot;','')"/>
				</xsl:variable>
				<xsl:variable name = "varImportTrueFalse" >
					<xsl:value-of select="translate(COL18,'&quot;','')"/>
				</xsl:variable>
				<xsl:if test="$varImportTrueFalse='Y'">
					<xsl:if test="$varInstrumentType='Equity' or $varInstrumentType='Option' or $varInstrumentType='CFD' ">
						<PositionMaster>
							<!--   Fund -->
							<!-- Column 20 (empty column) mapped with Fund hence fund always fixed-->
							<xsl:variable name = "varPortfolioID" >
								<xsl:value-of select="translate(COL20,'&quot;','')"/>
							</xsl:variable>

							<xsl:choose>
								<xsl:when test="$varPortfolioID='Letrol'">
									<FundName>
										<xsl:value-of select="'Letrol.002-36771-2'"/>
									</FundName>
								</xsl:when>
								<xsl:when test="$varPortfolioID='LETRP'">
									<FundName>
										<xsl:value-of select="'LETRP.11817245'"/>
									</FundName>
								</xsl:when>
								<xsl:when test="$varPortfolioID='LETRP2'">
									<FundName>
										<xsl:value-of select="'LETRP2.11802597'"/>
									</FundName>
								</xsl:when>
								<xsl:when test="$varPortfolioID='Al-Safi Main'">
									<FundName>
										<xsl:value-of select="'Al-Safi Main'"/>
									</FundName>
								</xsl:when>
								<xsl:otherwise>
									<FundName>
										<xsl:value-of select="'Letrol.002217487'"/>
									</FundName>
								</xsl:otherwise >
							</xsl:choose >

							<PBAssetType>
								<xsl:value-of select="translate(COL7,'&quot;','')"/>
							</PBAssetType>

							<xsl:choose>
								<xsl:when test ="COL19 &lt; 0 or COL19 &gt; 0 or COL19=0">
									<Commission>
										<xsl:value-of select="COL19"/>
									</Commission>
								</xsl:when>
								<xsl:otherwise>
									<Commission>
										<xsl:value-of select="0"/>
									</Commission>
								</xsl:otherwise>
							</xsl:choose>

							<!-- Column 4 mapped with Quantity,here for short positions qty value is -tive,so multiplied by (-1)-->
							<xsl:choose>
								<xsl:when test="COL4 &lt; 0">
									<NetPosition>
										<!--<xsl:value-of select='format-number(COL17*(-1),"###")'/>-->
										<xsl:value-of select="COL4*(-1)"/>
									</NetPosition>
								</xsl:when>
								<xsl:when test="COL4 &gt; 0">
									<NetPosition>
										<!--<xsl:value-of select='format-number(COL17*(-1),"###")'/>-->
										<xsl:value-of select="COL4"/>
									</NetPosition>
								</xsl:when>
								<xsl:otherwise>
									<NetPosition>
										<xsl:value-of select="0"/>
									</NetPosition>
								</xsl:otherwise>
							</xsl:choose >

							<xsl:choose>
								<xsl:when test ="COL11 &gt; 0 or COL11 &lt; 0 or COL11 = 0 ">
									<CostBasis>
										<xsl:value-of select="COL11"/>
									</CostBasis>
								</xsl:when>
								<xsl:otherwise>
									<CostBasis>
										<xsl:value-of select="0"/>
									</CostBasis>
								</xsl:otherwise>
							</xsl:choose>


							<!-- Position Date mapped with the column 16 -->
							<PositionStartDate>
								<xsl:value-of select="translate(COL1,'&quot;','')"/>
							</PositionStartDate>

							<xsl:variable name = "varOrderSide" >
								<xsl:value-of select="translate(COL3,'&quot;','')"/>
							</xsl:variable>
							<!-- Prime Broker Symbol -->

							<Symbol>
								<xsl:value-of select="translate(COL6,'&quot;','')"/>
							</Symbol>
							<PBSymbol>
								<xsl:value-of select="translate(COL5,'&quot;','')"/>
							</PBSymbol>

							<xsl:if test="$varInstrumentType='Equity' or $varInstrumentType='CFD' ">
								<xsl:choose>
									<xsl:when test="$varOrderSide='BUY'">
										<SideTagValue>
											<xsl:value-of select="'1'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='SELL'">
										<SideTagValue>
											<xsl:value-of select="'2'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='BUY TO CLOSE' or $varOrderSide='BUY CLOSE'">
										<SideTagValue>
											<xsl:value-of select="'B'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='SHORT SELL' or $varOrderSide='SELL SHORT'">
										<SideTagValue>
											<xsl:value-of select="'5'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:otherwise>
										<SideTagValue>
											<xsl:value-of select="''"/>
										</SideTagValue>
									</xsl:otherwise>
								</xsl:choose >
							</xsl:if >

							<xsl:if test="$varInstrumentType='Option'">
								<xsl:choose>
									<xsl:when test="$varOrderSide='BUY'">
										<SideTagValue>
											<xsl:value-of select="'A'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='SELL'">
										<SideTagValue>
											<xsl:value-of select="'D'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='BUY CLOSE' or $varOrderSide='BUY TO CLOSE'">
										<SideTagValue>
											<xsl:value-of select="'B'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="$varOrderSide='SHORT SELL' or $varOrderSide='SELL SHORT'">
										<SideTagValue>
											<xsl:value-of select="'C'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:otherwise>
										<SideTagValue>
											<xsl:value-of select="''"/>
										</SideTagValue>
									</xsl:otherwise>
								</xsl:choose >
							</xsl:if >

							<xsl:variable name = "varPBCounterParty" >
								<xsl:value-of select="translate(translate(COL12,'&quot;',''), $vLowercaseChars_CONST , $vUppercaseChars_CONST)"/>
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

						</PositionMaster>
					</xsl:if>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>