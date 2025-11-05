<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<PositionMaster>

					<!--<AccountName>
            <xsl:value-of select="''"/>
          </AccountName>-->

            		<!--<Symbol>
						<xsl:value-of select="COL6"/>
					</Symbol>

					<PBSymbol>
						<xsl:value-of select="COL8"/>
					</PBSymbol>

					<IDCOOptionSymbol>
						<xsl:choose>
							<xsl:when test ="string-length(COL6) = 21">
								<xsl:value-of select="concat(COL6, 'U')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</IDCOOptionSymbol>-->
			
					

					<xsl:variable name="PB_NAME">
						<xsl:value-of select="''"/>
					</xsl:variable>

					<xsl:variable name = "PB_SYMBOL_NAME" >
						<xsl:value-of select ="COL6"/>
					</xsl:variable>

					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
					</xsl:variable>

					<xsl:variable name ="Asset">
						<xsl:choose>
							<xsl:when test="string-length(COL6) = 21">
								<xsl:value-of select="'Option'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Equity'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="Symbol" select="COL6"/>

					<Symbol>
						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
								<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
							</xsl:when>
							<xsl:when test ="string-length(COL6) = 21">
								<xsl:value-of select ="''"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="COL6"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<IDCOOptionSymbol>
						<xsl:choose>
							<xsl:when test ="string-length(COL6) = 21">
								<xsl:value-of select="concat(COL6, 'U')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</IDCOOptionSymbol>



					<xsl:variable name = "PB_FUND_NAME">
						<xsl:value-of select="COL64"/>
					</xsl:variable>

					<xsl:variable name ="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='FIXTrade']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountName>
						<xsl:choose>

							<xsl:when test ="$PRANA_FUND_NAME!=''">
								<xsl:value-of select ="$PRANA_FUND_NAME"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</AccountName>




					<xsl:choose>
						<xsl:when test ="COL5 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL5 *(-1)"/>
							</NetPosition>
						</xsl:when >
						<xsl:when test ="COL5 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL5"/>
							</NetPosition>
						</xsl:when >
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose >


					<xsl:choose>
						<xsl:when test ="COL4='BTC' and (contains(COL8,'CALL ') or contains(COL8,'PUT '))">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL4='SSL' and (contains(COL8,'CALL ') or contains(COL8,'PUT '))">
							<SideTagValue>
								<xsl:value-of select="'C'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL4='BUY' and (contains(COL8,'CALL ') or contains(COL8,'PUT '))">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL4='SEL'  and (contains(COL8,'CALL ') or contains(COL8,'PUT '))">
							<SideTagValue>
								<xsl:value-of select="'D'"/>
							</SideTagValue>
						</xsl:when >
						
						<xsl:when test ="COL4='BUY'">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL4='SEL'">
							<SideTagValue>
								<xsl:value-of select="'2'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL4='SSL'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL4='BTC'">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
					

						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose >

					<xsl:variable name="Cost">
						<xsl:choose>
							<xsl:when test="number(COL5)">
								<xsl:value-of select="COL9"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>



					<xsl:choose>
						<xsl:when test ="boolean(number($Cost))">
							<CostBasis>
								<xsl:value-of select="$Cost"/>
							</CostBasis>
						</xsl:when>
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:variable name="Commission">
						
							<xsl:value-of select="COL12"/>
						
					</xsl:variable>

					<Commission>

						<xsl:choose>

							<xsl:when test="$Commission &gt; 0">
								<xsl:value-of select="$Commission"/>
							</xsl:when>

							<xsl:when test="$Commission &lt; 0">
								<xsl:value-of select="$Commission * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>

					</Commission>

					<!--<xsl:variable name="StampDuty">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL13"/>
						</xsl:call-template>
					</xsl:variable>

					<StampDuty>

						<xsl:choose>

							<xsl:when test="$StampDuty &gt; 0">
								<xsl:value-of select="$StampDuty"/>
							</xsl:when>

							<xsl:when test="$StampDuty &lt; 0">
								<xsl:value-of select="$StampDuty * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>

					</StampDuty>-->



					<xsl:variable name="Fees">
						
							<xsl:value-of select="COL19"/>
						
					</xsl:variable>

					<Fees>

						<xsl:choose>

							<xsl:when test="$Fees &gt; 0">
								<xsl:value-of select="$Fees"/>
							</xsl:when>

							<xsl:when test="$Fees &lt; 0">
								<xsl:value-of select="$Fees * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>

					</Fees>
					
					
					<xsl:variable name="PB_CounterParty" select="normalize-space(COL24)"/>
					<xsl:variable name="PRANA_CounterPartyCode">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='ML']/BrokerData[translate(@MLBroker,$vLowercaseChars_CONST,$vUppercaseChars_CONST)=$PB_CounterParty]/@PranaBrokerCode"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="$PRANA_CounterPartyCode !=''">
							<CounterPartyID>
								<xsl:value-of select="$PRANA_CounterPartyCode"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:otherwise>
							<CounterPartyID>
								<xsl:value-of select="5"/>
							</CounterPartyID>
						</xsl:otherwise>
					</xsl:choose>


					<PositionStartDate>
						<xsl:value-of select="COL2"/>
					</PositionStartDate>

					<!--<OriginalPurchaseDate>
						<xsl:value-of select="COL1"/>
					</OriginalPurchaseDate>-->

				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->


</xsl:stylesheet>
