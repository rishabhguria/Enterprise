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

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:param name="varPutCall"/>
		<xsl:choose>
			<xsl:when  test ="$varMonth='01' and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02' and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03' and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05' and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09' and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='10' and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='11' and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='12' and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='01' and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02' and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03' and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05' and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09' and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='10' and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='11' and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='12' and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/">
		
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="number(COL6) and COL4!='Cash'">
					<PositionMaster>

						<xsl:variable name ="varUnderlyingSymbol">
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select ="substring-before(normalize-space(COL7),' ')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExpireDate">
							
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select="substring(substring-after(normalize-space(COL7),' '),1,6)"/>
								</xsl:when>
							</xsl:choose>
						
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select="substring($varExpireDate,1,2)"/>
								</xsl:when>
							</xsl:choose>
							
						</xsl:variable>

						<xsl:variable name="varMonth">

							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select="substring($varExpireDate,3,2)"/>
								</xsl:when>
							</xsl:choose>
							
						</xsl:variable>

						<xsl:variable name="varExDay">
							<xsl:value-of select="substring($varExpireDate,5,2)"/>
						</xsl:variable>


						<xsl:variable name="varPutCall">
							
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select ="substring(substring-after(normalize-space(COL7),' '),7,1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varStrikePrice">
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select="substring-after(COL8,'@')"/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>					

						<xsl:variable name ="varMonthCode">
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="$varMonth"/>
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
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select='my:Now(number(concat(20,$varYear)),number($varMonth))'/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>

						<xsl:variable name="varOptionSymbol">
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:choose>
										<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>	
						

						<ExpirationDate>
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select="concat($varMonth,'/',$varDays,'/20',$varYear)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ExpirationDate>

						<TickerSymbol>							
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select="$varOptionSymbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="normalize-space(COL7)"/>
								</xsl:otherwise> 
							</xsl:choose>
							
						</TickerSymbol>

						<xsl:choose>
							<xsl:when test ="contains(COL5,'Option')">
								<IDCOOptionSymbol>
									<xsl:value-of select ="concat(COL7,'U')"/>
								</IDCOOptionSymbol>
								<OSIOptionSymbol>
									<xsl:value-of select ="COL7"/>
								</OSIOptionSymbol>
							</xsl:when>
							<xsl:otherwise>
								<IDCOOptionSymbol>
									<xsl:value-of select ="''"/>
								</IDCOOptionSymbol>
								<OSIOptionSymbol>
									<xsl:value-of select ="''"/>
								</OSIOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>

						<AUECID>
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select="'12'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'1'"/>
								</xsl:otherwise>
							</xsl:choose>
							
						
						</AUECID>

						<LongName>
							<xsl:value-of select="COL8"/>
						</LongName>

						<PBSymbol>
							<xsl:value-of select="COL8"/>
						</PBSymbol>

						<UnderLyingSymbol>
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select ="substring-before(normalize-space(COL7),' ')"/>
								</xsl:when>
							</xsl:choose>
						</UnderLyingSymbol>
				

						<PutOrCall>
							<xsl:choose>
								<xsl:when test ="$varPutCall='P'">
									<xsl:value-of select ="'0'"/>
								</xsl:when >
								<xsl:when test ="$varPutCall='C'">
									<xsl:value-of select ="'1'"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select ="'2'"/>
								</xsl:otherwise>
							</xsl:choose>
						</PutOrCall>

						<StrikePrice>
							
							<xsl:choose>
								<xsl:when test =" contains(COL5,'Options')">
									<xsl:value-of select="format-number($varStrikePrice,'#.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
								<!--<xsl:value-of select="$varStrikePrice"/>-->

						</StrikePrice>

						<Multiplier>
							<xsl:value-of select="100"/>
						</Multiplier>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
