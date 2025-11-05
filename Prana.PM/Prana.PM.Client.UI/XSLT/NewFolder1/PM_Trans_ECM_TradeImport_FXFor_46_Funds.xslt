<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">
				<xsl:variable name = "PB_FUND_NAME" >
					<xsl:value-of select="(COL1)"/>
				</xsl:variable>

				<xsl:variable name="PRANA_FUND_NAME">
					<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='CustomHouse']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>
				<!--<xsl:if test ="((COL1 = '0E302' or COL1 = '0E303' or COL1='0E310' or COL1='0E311' or COL1='0E322' or COL1='0E340' or COL1='0E341' or COL1='0E342' or COL1='0E348' or COL1='0E354') and COL6 = 'S9' and COL2 != 'C'  and COL2 != 'E'  and COL2 != 'X' and COL2 != 'B'  and COL2 != 'S' ) ">-->
				<xsl:if test ="(COL6 = 'S9' and $PRANA_FUND_NAME !='' and COL2 != 'C'  and COL2 != 'B' and COL2 != 'E'  and COL2 != 'X' and COL2 != 'S' ) ">

					<!--Change by ashish for filter  cancel trade -->
					<!--<xsl:if test="contains(normalize-space(COL21),'CANCEL')= false">-->
					<PositionMaster>

						<!--fundname section-->
						

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select="''"/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test ="COL6 = 'S9'">
								<Symbol>
									<xsl:value-of select ="concat(COL8,'-',COL9,' ',COL25)"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select ="concat(COL8,'-',COL9,' ',COL25)"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>


						<xsl:choose>
							<xsl:when test ="COL2='B'">
								<xsl:choose>
									<xsl:when test="COL11 &gt; 0">
										<SideTagValue>
											<xsl:value-of select="'1'"/>
										</SideTagValue>
									</xsl:when>

									<xsl:otherwise>
										<SideTagValue>
											<xsl:value-of select="'2'"/>
										</SideTagValue>
									</xsl:otherwise>
								</xsl:choose>

							</xsl:when>

							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="COL7='BUY'">
										<SideTagValue>
											<xsl:value-of select="'1'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:when test="COL7 ='SELL'">
										<SideTagValue>
											<xsl:value-of select="'2'"/>
										</SideTagValue>
									</xsl:when>
									<xsl:otherwise>
										<SideTagValue>
											<xsl:value-of select="''"/>
										</SideTagValue>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>



						<xsl:choose>
							<xsl:when test="COL6='S9'">
								<xsl:choose>
									<xsl:when test="COL11 &lt; 0">
										<NetPosition>
											<xsl:value-of select="COL11 * (-1)"/>
										</NetPosition>
									</xsl:when>
									<xsl:when test="COL11 &gt; 0">
										<NetPosition>
											<xsl:value-of select="COL11"/>
										</NetPosition>
									</xsl:when>
									<xsl:otherwise>
										<NetPosition>
											<xsl:value-of select="0"/>
										</NetPosition>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>



						<xsl:choose>
							<xsl:when test ="COL3='Trade Date' or COL3=''">
								<PositionStartDate>
									<xsl:value-of select="''"/>
								</PositionStartDate>
								<AUECLocalDate>
									<xsl:value-of select="''"/>
								</AUECLocalDate>
							</xsl:when>
							<xsl:otherwise>
								<PositionStartDate>
									<xsl:value-of select="concat(substring(COL3,5,2),'/',substring(COL3,7,2),'/',substring(COL3,1,4))"/>
								</PositionStartDate>
								<AUECLocalDate>
									<xsl:value-of select="concat(substring(COL3,5,2),'/',substring(COL3,7,2),'/',substring(COL3,1,4))"/>
								</AUECLocalDate>
							</xsl:otherwise>
						</xsl:choose>




						<xsl:choose>
							<xsl:when test ="COL25='Settlement Date' or COL25='*'">
								<PositionSettlementDate>
									<xsl:value-of select="''"/>
								</PositionSettlementDate>
							</xsl:when>
							<xsl:otherwise>
								<PositionSettlementDate>
									<xsl:value-of select="concat(substring(COL25,5,2),'/',substring(COL25,7,2),'/',substring(COL25,1,4))"/>
								</PositionSettlementDate>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name = "varMulTiply" >
							<xsl:value-of select="(COL11 * COL13)"/>
						</xsl:variable>


						<xsl:variable name = "varQTY1PRICE" >
							<xsl:choose>
								<xsl:when test ="(COL11 * COL13) &lt; 0">
									<xsl:value-of select ="(COL11 * COL13) * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="(COL11 * COL13)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name = "varQTY2" >
							<xsl:choose>
								<xsl:when test ="COL12 &lt; 0">
									<xsl:value-of select ="COL12 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL12"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name = "varQTY1" >
							<xsl:choose>
								<xsl:when test ="COL11 &lt; 0">
									<xsl:value-of select ="COL11 * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL11"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="COL2='B'">
								<PBSymbol>
									<xsl:value-of select ="concat('CANCEL, Qt1- ',COL11, ', Price- ',COL13,', Qt2- ',COL12,', Qt1*Price-',$varQTY1PRICE,',QT2 = QT1 div Price -', $varQTY1PRICE  div COL13)"/>
									<!--<xsl:value-of select ="''"/>-->
								</PBSymbol>
							</xsl:when>
							<xsl:otherwise>
								<PBSymbol>
									<!--<xsl:value-of select ="concat(COL8,'-',COL9,' ',COL25)"/>-->
									<xsl:value-of select ="concat('Qt1- ',COL11, ', Price- ',COL13,', Qt2- ',COL12,', Qt1*Price-',$varQTY1PRICE,',QT2 = QT1 div Price -', $varQTY1PRICE  div COL13)"/>

								</PBSymbol>
							</xsl:otherwise>
						</xsl:choose>


						<!--<xsl:choose>
							<xsl:when  test="round(number($varQTY2)) = round(number($varQTY1PRICE)) and number(COL13)">
								<CostBasis>
									-->
						<!--<xsl:value-of select="COL13"/>-->
						<!--
									<xsl:value-of select="varQTY2 div varQTY1"/>
								</CostBasis>
							</xsl:when >
							<xsl:when  test="round(number($varQTY2)) != round(number($varQTY1PRICE)) and number(COL13) and COL13 != 0">
								<CostBasis>
									<xsl:value-of select="1 div COL13"/>
								</CostBasis>
							</xsl:when >
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose >-->
						<xsl:variable name = "varPrice" >
							<xsl:value-of select="$varQTY2 div $varQTY1"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test ="$varPrice &gt; 0">
								<CostBasis>
									<xsl:value-of select="$varPrice"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select= '0'/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

					</PositionMaster>
				</xsl:if>
				<!--</xsl:if>-->
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->




</xsl:stylesheet>