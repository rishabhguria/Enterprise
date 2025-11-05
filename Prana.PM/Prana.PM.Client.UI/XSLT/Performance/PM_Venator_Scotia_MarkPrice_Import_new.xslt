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

	<xsl:template name ="MonthCode">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>
			<xsl:when  test ="$varMonth=1 and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test =" $varMonth=10 and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12  and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=1 and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=10 and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12 and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">

		<xsl:param name="varSymbol"/>

		<xsl:param name="varSecurityType"/>

		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:choose>
				<xsl:when test ="$varSecurityType='OPTION'">
					<xsl:value-of select="substring-before($varSymbol,' ')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:choose>
				<xsl:when test ="$varSecurityType='OPTION'">
					<xsl:value-of select ="number(substring(substring-after($varSymbol,' '),1,2))"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:choose>
				<xsl:when test ="$varSecurityType='OPTION'">
					<xsl:value-of select ="number(substring(substring-after($varSymbol,' '),3,2))"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:choose>
				<xsl:when test ="$varSecurityType='OPTION'">
					<xsl:value-of select ="substring(substring-after($varSymbol,' '),5,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:choose>
				<xsl:when test ="$varSecurityType='OPTION'">
					<xsl:value-of select ="substring(substring-after($varSymbol,' '),7,1)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePriceInt">
			<xsl:choose>
				<xsl:when test ="$varSecurityType='OPTION'">
					<xsl:value-of select ="substring(substring-after($varSymbol,' '),8,5)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePriceDec">
			<xsl:choose>
				<xsl:when test ="$varSecurityType='OPTION'">
					<xsl:value-of select ="substring(substring-after($varSymbol,' '),13,3)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePrice">
			<xsl:choose>
				<xsl:when test="number($varStrikePriceInt) or number($varStrikePriceDec)">
					<xsl:value-of select ="format-number(concat($varStrikePriceInt,'.',$varStrikePriceDec),'#.00')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varMonthCode">
			<xsl:call-template name ="MonthCode">
				<xsl:with-param name ="varMonth" select ="number($varMonth)"/>
				<xsl:with-param name ="varPutCall" select="$varPutCall"/>
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
				<xsl:when test ="$varSecurityType='OPTION' and number($varYear) and number($varMonth)">
					<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>


		<xsl:choose>
			<xsl:when test ="$varSecurityType='OPTION'">
				<xsl:choose>
					<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
						<xsl:choose>
							<xsl:when test="COL18!=1">
								<xsl:value-of select="concat('O:',$varUnderlyingSymbol,'-TC',' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="COL18!=1">
								<xsl:value-of select="concat('O:',$varUnderlyingSymbol,'-TC',' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="Asset" select="normalize-space(COL7)"/>

				<xsl:variable name="MarkPrice">
					<xsl:choose>
						<xsl:when test="$Asset='COMMON' or $Asset='ADR' or $Asset='WARRANT' or $Asset='ETF' or $Asset='REIT' or $Asset='CORP' or $Asset='CONV BOND'">
							<xsl:value-of select="translate(COL23,',','') div (translate(COL8,',','') * (translate(COL18,',','')))"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="COL15"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>



				<xsl:if test ="number($MarkPrice)">

					<PositionMaster>

						<xsl:variable name="PB_NAME" select="'PFR'"/>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:choose>
								<xsl:when test="COL7='CORP'">
									<xsl:value-of select="concat(COL4,' ',COL20)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL4"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="CUSIP" select="COL9"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<!--<xsl:choose>
								<xsl:when test="$Asset='OPTION'">-->
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$CUSIP]/@PranaSymbol"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</xsl:variable>


						<xsl:variable name="varSuffix">
							<xsl:choose>
								<xsl:when test="contains(substring-after(COL15,' '),'CT') or contains(substring-after(COL15,' '),'CN') or contains(substring(COL15,string-length(COL15)-1),'CN') or contains(substring(COL15,string-length(COL15)-1),'CT')">
									<xsl:value-of select="'CAD'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'USD'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME" select="$varSuffix"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@TickerSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varOptionTicker">
							<xsl:call-template name="Option">
								<xsl:with-param name="varSymbol" select="normalize-space(COL30)"/>
								<xsl:with-param name="varSecurityType" select="$Asset"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varSymbol" select="substring-before(COL20,' ')"/>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test ="$Asset='OPTION'">
									<xsl:value-of select ="$varOptionTicker"/>
								</xsl:when>

								<xsl:when test="contains(COL7,'BOND') or contains(COL7,'CORP')">
									<xsl:value-of select="COL9"/>
								</xsl:when>
								
								<xsl:when test="normalize-space(COL7)='RIGHTS'">
									<xsl:value-of select="'TRC/R'"/>
								</xsl:when>
								<!--<xsl:when test="$Asset='CONV BOND'">
									<xsl:value-of select="''"/>
								</xsl:when>-->

								<!--<xsl:when test="COL5='CORP'">
									<xsl:value-of select="normalize-space(COL7)"/>
								</xsl:when>-->

								<xsl:when test="substring-after(COL20,' ')='CV'">
									<xsl:value-of select="concat(substring-before(COL20,' '),'-VC')"/>
								</xsl:when>

								<xsl:when test="contains(COL20,'/') and substring-after(COL20,' ')='UN'">
									<xsl:value-of select="translate(COL26,'/','.')"/>
								</xsl:when>

								<xsl:when test ="COL19='CAD'">
									<xsl:choose>
										<xsl:when test="contains($varSymbol,'/')">
											<xsl:value-of select="concat(substring-before($varSymbol,'/'),'.',substring(substring-after($varSymbol,'/'),1,1),'-TC')"/>
										</xsl:when>
										<xsl:when test="contains($varSymbol,'-')">
											<xsl:choose>
												<xsl:when test="substring(substring-after($varSymbol,'-'),1,1)='U'">
													<xsl:value-of select="concat(substring-before($varSymbol,'-'),'.','UN','-TC')"/>
												</xsl:when>
												<xsl:when test="substring(substring-after($varSymbol,'-'),1,1)='W'">
													<xsl:value-of select="concat(substring-before($varSymbol,'-'),'.','WT','-TC')"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="contains(substring($varSymbol,string-length($varSymbol)-1),'US')">
											<xsl:value-of select="substring($varSymbol,1,string-length($varSymbol)-2)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="contains(substring($varSymbol,string-length($varSymbol)-1),'CN') or contains(substring($varSymbol,string-length($varSymbol)-1),'CT')">
													<xsl:value-of select ="concat(substring($varSymbol,1,string-length($varSymbol)-2),'-TC')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select ="concat(normalize-space($varSymbol),'-TC')"/>
												</xsl:otherwise>
											</xsl:choose>

										</xsl:otherwise>
									</xsl:choose>

								</xsl:when>
								<xsl:when test="COL19!='CAD'">
									<xsl:choose>
										<xsl:when test="contains($varSymbol,'/')">
											<xsl:value-of select="concat(substring-before($varSymbol,'/'),'.',substring(substring-after($varSymbol,'/'),1,1),'-TC')"/>
										</xsl:when>
										<xsl:when test="contains($varSymbol,'-')">
											<xsl:choose>
												<xsl:when test="substring(substring-after($varSymbol,'-'),1,1)='U'">
													<xsl:value-of select="concat(substring-before($varSymbol,'-'),'.','UN','-TC')"/>
												</xsl:when>
												<xsl:when test="substring(substring-after($varSymbol,'-'),1,1)='W'">
													<xsl:value-of select="concat(substring-before($varSymbol,'-'),'.','WT','-TC')"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="contains(substring($varSymbol,string-length($varSymbol)-1),'US')">
											<xsl:value-of select="substring($varSymbol,1,string-length($varSymbol)-2)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="contains(substring($varSymbol,string-length($varSymbol)-1),'CN') or contains(substring($varSymbol,string-length($varSymbol)-1),'CT')">
													<xsl:value-of select ="concat(substring($varSymbol,1,string-length($varSymbol)-2),'-TC')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select ="normalize-space($varSymbol)"/>
												</xsl:otherwise>
											</xsl:choose>

										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<!--<CUSIP>
							<xsl:choose>
								<xsl:when test ="$Asset='CONV BOND'">
									<xsl:value-of select="COL9"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>-->

						<MarkPrice>
							<!--<xsl:choose>
								<xsl:when test="$Asset='OPTION'">
									<xsl:choose>

										<xsl:when test="$MarkPrice &gt; 0">
											<xsl:value-of select="$MarkPrice div 100"/>
										</xsl:when>

										<xsl:when test="$MarkPrice &lt; 0">
											<xsl:value-of select="($MarkPrice*-1) div 100"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>-->
							<xsl:choose>

								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>
								</xsl:when>

								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice*-1"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
							<!--</xsl:otherwise>
							</xsl:choose>-->
						</MarkPrice>

						<Date>
							<xsl:value-of select="COL1"/>
						</Date>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->

</xsl:stylesheet>


