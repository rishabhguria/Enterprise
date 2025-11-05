<?xml version="1.0" encoding="utf-8"?>
<!--
Refer to the Altova MapForce 2007 Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<PositionMaster>
					<!--   Fund -->
					<!-- Column 1 mapped with Fund-->
					<xsl:variable name = "varPortfolioID" >
						<xsl:value-of select="translate(COL1,'&quot;','')"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$varPortfolioID='4RWX0409' or $varPortfolioID = '4RWXF909' or $varPortfolioID = '4RWX1209' or $varPortfolioID = '4RWXF919' or $varPortfolioID = '4RWX5519'">
							<FundName>
								<xsl:value-of select="'WProp'"/>
							</FundName>
						</xsl:when>
						<xsl:otherwise>
							<FundName>
								<xsl:value-of select="' '"/>
							</FundName>
						</xsl:otherwise >
					</xsl:choose >

					<xsl:choose>
						<xsl:when test ="COL3 !='EQUITY' and COL26 != 'AUECID' and COL27 !='AssetID' and COL28 != 'UnderlyingID' and COL29 !='ExchangeID' and COL30 != 'CurrencyID'">
							<AUECID>
								<xsl:value-of select ="COL26"/>
							</AUECID>
							<UnderlyingID>
								<xsl:value-of select ="COL28"/>
							</UnderlyingID>
							<ExchangeID>
								<xsl:value-of select ="COL29"/>
							</ExchangeID>
							<CurrencyID>
								<xsl:value-of select ="1"/>
							</CurrencyID>
							<AssetID>
								<xsl:value-of select ="COL27"/>
							</AssetID>
						</xsl:when>
						<xsl:otherwise>
							<AUECID>
								<xsl:value-of select ="0"/>
							</AUECID>
							<UnderlyingID>
								<xsl:value-of select ="0"/>
							</UnderlyingID>
							<ExchangeID>
								<xsl:value-of select ="0"/>
							</ExchangeID>
							<CurrencyID>
								<xsl:value-of select ="0"/>
							</CurrencyID>
							<AssetID>
								<xsl:value-of select ="0"/>
							</AssetID>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test ="COL9='Trade Date' or COL9='*'">
							<PositionStartDate>
								<xsl:value-of select="''"/>
							</PositionStartDate>
							<AUECLocalDate>
								<xsl:value-of select="''"/>
							</AUECLocalDate>
						</xsl:when>
						<xsl:otherwise>
							<PositionStartDate>
								<xsl:value-of select="translate(COL9,'&quot;','')"/>
							</PositionStartDate>
							<AUECLocalDate>
								<xsl:value-of select="translate(COL9,'&quot;','')"/>
							</AUECLocalDate>
						</xsl:otherwise>
					</xsl:choose>

					<!-- Settlement Date-->
					<xsl:choose>
						<xsl:when test ="COL24='Settle Date/ValueDate' or COL24='*'">
							<PositionSettlementDate>
								<xsl:value-of select="''"/>
							</PositionSettlementDate>
						</xsl:when>
						<xsl:otherwise>
							<PositionSettlementDate>
								<xsl:value-of select="translate(COL24,'&quot;','')"/>
							</PositionSettlementDate>
						</xsl:otherwise>
					</xsl:choose>


					<!-- Side-->
					<xsl:choose>
						<xsl:when test ="COL25 ='BUY  TO CLOSE' or COL25 ='BUY TO COVER'">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test ="COL25 = 'BUY'">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test ="COL25 = 'SELL' or COL25 = 'SELL TO CLOSE'">
							<SideTagValue>
								<xsl:value-of select="'2'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test ="COL25 = 'SHORT SELL' or COL25 = 'SELL SHORT'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:when test ="COL25 = 'SELL TO OPEN'">
							<SideTagValue>
								<xsl:value-of select="'C'"/>
							</SideTagValue>
						</xsl:when>
						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose>

					<!-- Quantity-->
					<xsl:choose>
						<xsl:when test ="COL10 &gt; 0 or COL10 = 0">
							<NetPosition>
								<xsl:value-of select="COL10"/>
							</NetPosition>
						</xsl:when>
						<xsl:when test ="COL10 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL10 * (-1)"/>
							</NetPosition>
						</xsl:when>
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose>

					<!-- Average Price-->
					<xsl:choose>
						<xsl:when test ="COL11 &lt; 0 or COL11 &gt; 0 or COL11=0">
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

					<xsl:variable name = "varInstrumentType" >
						<xsl:value-of select="translate(translate(COL3, ' ' , ''),'&quot;','')"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test ="$varInstrumentType='EQUITY'">
							<Symbol>
								<xsl:value-of select="translate(COL5,'&quot;','')"/>
							</Symbol>
						</xsl:when>
						<xsl:when test ="$varInstrumentType='OPTION'">
							<Symbol>
								<xsl:value-of select="translate(COL7,'&quot;','')"/>
							</Symbol>
						</xsl:when>
						<xsl:when test ="$varInstrumentType='FUTURE'">
							<xsl:variable name = "varLength" >
								<xsl:value-of select="string-length(translate(translate(COL6,'&quot;',''),' ',''))"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test ="$varLength &gt; 0 ">
									<xsl:variable name = "varAfter" >
										<xsl:value-of select="substring(COL6,($varLength)-1,2)"/>
									</xsl:variable>
									<xsl:variable name = "varBefore" >
										<xsl:value-of select="substring(COL6,1,($varLength)-2)"/>
									</xsl:variable>
									<Symbol>
										<xsl:value-of select="concat($varBefore,' ',$varAfter)"/>
									</Symbol>
								</xsl:when>
								<xsl:otherwise>
									<Symbol>
										<xsl:value-of select="''"/>
									</Symbol>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
						</xsl:otherwise>
					</xsl:choose>

					<PBAssetType>
						<xsl:value-of select="COL3"/>
					</PBAssetType>
					<xsl:choose>
						<xsl:when test ="$varInstrumentType='EQUITY'">
							<PBSymbol>
								<xsl:value-of select="translate(COL5,'&quot;','')"/>
							</PBSymbol>
						</xsl:when>
						<xsl:when test ="$varInstrumentType='OPTION'">
							<PBSymbol>
								<xsl:value-of select="translate(COL7,'&quot;','')"/>
							</PBSymbol>
						</xsl:when>
						<xsl:when test ="$varInstrumentType='FUTURE'">
							<PBSymbol>
								<xsl:value-of select="COL6"/>
							</PBSymbol>
						</xsl:when>
						<xsl:otherwise>
							<PBSymbol>
								<xsl:value-of select="COL4"/>
							</PBSymbol>
						</xsl:otherwise>
					</xsl:choose>



					<!--Fees   -->
					<xsl:choose>
						<xsl:when test ="COL17 &gt; 0 or COL17=0">
							<Fees>
								<xsl:value-of select="COL17"/>
							</Fees>
						</xsl:when>
						<xsl:when test ="COL17 &lt; 0 ">
							<Fees>
								<xsl:value-of select="COL17 * (-1)"/>
							</Fees>
						</xsl:when>
						<xsl:otherwise>
							<Fees>
								<xsl:value-of select="0"/>
							</Fees>
						</xsl:otherwise>
					</xsl:choose>

					<!--Commission   -->
					<xsl:choose>
						<xsl:when test ="COL15 &gt; 0 or COL15=0">
							<Commission>
								<xsl:value-of select="COL15"/>
							</Commission>
						</xsl:when>
						<xsl:when test ="COL15 &lt; 0 ">
							<Commission>
								<xsl:value-of select="COL15*(-1)"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>

					<!--Stamp Duty   -->
					<xsl:choose>
						<xsl:when test ="COL20 &gt; 0 or COL20=0">
							<StampDuty>
								<xsl:value-of select="COL20"/>
							</StampDuty>
						</xsl:when>
						<xsl:when test ="COL20 &lt; 0">
							<StampDuty>
								<xsl:value-of select="COL20 * (-1)"/>
							</StampDuty>
						</xsl:when>
						<xsl:otherwise>
							<StampDuty>
								<xsl:value-of select="0"/>
							</StampDuty>
						</xsl:otherwise>
					</xsl:choose>

					<!--Trasanction Levy   -->
					<xsl:choose>
						<xsl:when test ="COL16 &gt; 0 or COL16=0">
							<TransactionLevy>
								<xsl:value-of select="COL16"/>
							</TransactionLevy>
						</xsl:when>
						<xsl:when test ="COL16 &lt; 0 ">
							<TransactionLevy>
								<xsl:value-of select="COL16*(-1)"/>
							</TransactionLevy>
						</xsl:when>
						<xsl:otherwise>
							<TransactionLevy>
								<xsl:value-of select="0"/>
							</TransactionLevy>
						</xsl:otherwise>
					</xsl:choose>

					<!--Cleaing Fees   -->
					<xsl:choose>
						<xsl:when test ="COL18 &gt; 0 or COL18=0">
							<ClearingFee>
								<xsl:value-of select="COL18"/>
							</ClearingFee>
						</xsl:when>
						<xsl:when test ="COL18 &lt; 0">
							<ClearingFee>
								<xsl:value-of select="COL18 * (-1)"/>
							</ClearingFee>
						</xsl:when>
						<xsl:otherwise>
							<ClearingFee>
								<xsl:value-of select="0"/>
							</ClearingFee>
						</xsl:otherwise>
					</xsl:choose>

					<!--Misc Fees   -->
					<xsl:choose>
						<xsl:when test ="boolean(number(COL13))">
							<MiscFees>
								<xsl:value-of select="(number(translate(COL13,'-',''))) + (number(translate(COL14,'-',''))) + (number(translate(COL19,'-',''))) + (number(translate(COL22,'-','')))"/>
							</MiscFees>
						</xsl:when>
						<xsl:otherwise>
							<MiscFees>
								<xsl:value-of select="0"/>
							</MiscFees>
						</xsl:otherwise>
					</xsl:choose>

				</PositionMaster>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>
