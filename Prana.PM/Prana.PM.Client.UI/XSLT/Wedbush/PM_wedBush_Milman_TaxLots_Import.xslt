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

	<xsl:template name="MonthNo">
		<xsl:param name="varMonthCode"/>
		<xsl:param name="varPutCall"/>

		<xsl:choose>
			<xsl:when test ="$varMonthCode='A' and $varPutCall='C'">
				<xsl:value-of select ="1"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='B' and $varPutCall='C'">
				<xsl:value-of select ="2"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='C' and $varPutCall='C'">
				<xsl:value-of select ="3"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='D' and $varPutCall='C'">
				<xsl:value-of select ="4"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='E' and $varPutCall='C'">
				<xsl:value-of select ="5"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='F' and $varPutCall='C'">
				<xsl:value-of select ="6"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='G' and $varPutCall='C'">
				<xsl:value-of select ="7"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='H' and $varPutCall='C'">
				<xsl:value-of select ="8"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='I' and $varPutCall='C'">
				<xsl:value-of select ="9"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='J' and $varPutCall='C'">
				<xsl:value-of select ="10"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='K' and $varPutCall='C'">
				<xsl:value-of select ="11"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='L' and $varPutCall='C'">
				<xsl:value-of select ="12"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='M' and $varPutCall='P'">
				<xsl:value-of select ="1"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='N' and  $varPutCall='P'">
				<xsl:value-of select ="2"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='O' and  $varPutCall='P'">
				<xsl:value-of select ="3"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='P' and  $varPutCall='P'">
				<xsl:value-of select ="4"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='Q' and  $varPutCall='P'">
				<xsl:value-of select ="5"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='R' and  $varPutCall='P'">
				<xsl:value-of select ="6"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='S' and  $varPutCall='P'">
				<xsl:value-of select ="7"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='T' and  $varPutCall='P'">
				<xsl:value-of select ="8"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='U' and  $varPutCall='P'">
				<xsl:value-of select ="9"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='V' and  $varPutCall='P'">
				<xsl:value-of select ="10"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='W' and  $varPutCall='P'">
				<xsl:value-of select ="11"/>
			</xsl:when>
			<xsl:when test ="$varMonthCode='X' and  $varPutCall='P'">
				<xsl:value-of select ="12"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="varSymbol"/>


		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:choose>
				<xsl:when test="substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL'">
					<xsl:value-of select ="substring-before($varSymbol,'1')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:choose>
				<xsl:when test="substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL'">
					<xsl:value-of select ="concat('1',substring(normalize-space(substring-after($varSymbol,'1')),1,1))"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:choose>
				<xsl:when test="substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL'">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,'1')),2,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:choose>
				<xsl:when test="substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL'">
					<xsl:value-of select ="substring(COL5,1,1)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePrice1">
			<xsl:choose>
				<xsl:when test="substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL'">
					<xsl:value-of select ="number(substring-before(normalize-space(substring-after(COL5,'$')),' '))"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varStrikePrice">
			<xsl:choose>
				<xsl:when test="number($varStrikePrice1)">
					<xsl:value-of select="format-number($varStrikePrice1,'#.00')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varMonthCode">
			<xsl:choose>
				<xsl:when test="substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL'">
					<xsl:value-of select ="substring(normalize-space(substring-after($varSymbol,'1')),4,1)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:call-template name="MonthNo">
				<xsl:with-param name="varMonthCode" select="$varMonthCode"/>
				<xsl:with-param name="varPutCall" select="$varPutCall"/>
			</xsl:call-template>
		</xsl:variable>


		<xsl:variable name="varDays">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)='0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--<xsl:variable name="varThirdFriday">

			<xsl:choose>
				<xsl:when test="substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL' and number($varYear) and number($varMonth)">
					<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>

		--><!--<xsl:value-of select ="concat($varUnderlyingSymbol,' ',number(substring-before(normalize-space(substring-after(COL5,'$')),' ')))"/>--><!--


		<xsl:choose>
			<xsl:when test="substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL'">
				<xsl:choose>
					<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice)"/>
					</xsl:when>
					<xsl:otherwise>-->
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice,'D',$varDays)"/>
					<!--</xsl:otherwise>
				</xsl:choose>-->
			<!--</xsl:when>
		</xsl:choose>-->

	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL2)">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'WED'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name="varUnderlyingSymbol">
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL') and contains(COL4,'14')">
									<xsl:value-of select="substring-before($varSymbol,'1')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL') and contains(COL4,'14')">
									<xsl:value-of select="concat('1',substring(normalize-space(substring-after($varSymbol,'1')),1,1))"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExDay">
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL') and contains(COL4,'14')">
									<xsl:value-of select="substring(normalize-space(substring-after($varSymbol,'1')),2,2)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varPutCall">
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL') and contains(COL4,'14')">
									<xsl:value-of select="substring(COL5,1,1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varStrikePrice1">
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL') and contains(COL4,'14')">
									<xsl:value-of select="number(substring-before(normalize-space(substring-after(COL5,'$')),' '))"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varStrikePrice">
							<xsl:choose>
								<xsl:when test="number($varStrikePrice1)">
									<xsl:value-of select="format-number($varStrikePrice1,'#.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varMonthCode">
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL') and contains(COL4,'14')">
									<xsl:value-of select="substring(normalize-space(substring-after($varSymbol,'1')),4,1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:call-template name="MonthNo">
								<xsl:with-param name="varMonthCode" select="$varMonthCode"/>
								<xsl:with-param name="varPutCall" select="$varPutCall"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varDays">
							<xsl:choose>
								<xsl:when test="substring($varExDay,1,1)='0'">
									<xsl:value-of select="substring($varExDay,2,1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varExDay"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varThirdFriday">
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL') and contains(COL4,'14') and number($varYear) and number($varMonth)">
									<xsl:value-of select="my:Now(concat(20,$varYear),$varMonth)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="TickerSymbol">
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,3)='CAL') and contains(COL4,'14')">
									<xsl:choose>
										<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice,'D',$varDays)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="COL4!='*'">
									<xsl:choose>
										<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,4)='CALL') and contains(COL4,'14')">
											<xsl:value-of select="$TickerSymbol"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="COL3"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>					
								
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

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

						<PositionStartDate>
							<xsl:value-of select ="COL7"/>
						</PositionStartDate>

						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="COL2"/>
						</xsl:variable>

						<NetPosition>

							<xsl:choose>

								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetPosition>

						<xsl:variable name="varSide" select="COL1"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,4)='CALL') and contains(COL4,'14')">
									<xsl:choose>

										<xsl:when test="$varSide='B'">
											<xsl:value-of select="'A'"/>
										</xsl:when>

										<xsl:when test="$varSide='Sell'">
											<xsl:value-of select="'D'"/>
										</xsl:when>

										<xsl:when test="$varSide='S'">
											<xsl:value-of select="'C'"/>
										</xsl:when>

										
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test="$varSide='L'">
											<xsl:value-of select="'1'"/>
										</xsl:when>

										<xsl:when test="$varSide='S'">
											<xsl:value-of select="'2'"/>
										</xsl:when>

										<xsl:when test="$varSide='Short'">
											<xsl:value-of select="'5'"/>
										</xsl:when>

										<xsl:when test="$varSide='Cover'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<xsl:variable name="Amt">											
							<xsl:choose>
								<xsl:when test ="COL8 &lt;0">
									<xsl:value-of select ="COL8*-1"/>
								</xsl:when>
								<xsl:when test ="COL8 &gt;0">
									<xsl:value-of select ="COL8"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="COM">
							<xsl:choose>
								<xsl:when test ="COL14 &lt;0">
									<xsl:value-of select ="COL14*-1"/>
								</xsl:when>
								<xsl:when test ="COL14 &gt;0">
									<xsl:value-of select ="COL14"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Commission>
							<xsl:value-of select ="$COM"/>
						</Commission>
						
						

						<xsl:variable name ="varCostBasis">
							<xsl:choose>
								<xsl:when test="(substring(COL5,1,3)='PUT' or substring(COL5,1,4)='CALL') and contains(COL4,'14')">
									<xsl:choose>
										<xsl:when test="$varSide='Buy' or $varSide='Cover'">
											<xsl:value-of select="((number($Amt)-number($COM)) div number(COL3)) div 100"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="((number($Amt)+number($COM)) div number(COL3)) div 100"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								
								<xsl:otherwise>
									
									<xsl:choose>
										<xsl:when test="$varSide='Buy' or $varSide='Cover'">
											<xsl:value-of select="(number($Amt)-number($COM)) div number(COL3)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="(number($Amt)+number($COM)) div number(COL3)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>							
							
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       